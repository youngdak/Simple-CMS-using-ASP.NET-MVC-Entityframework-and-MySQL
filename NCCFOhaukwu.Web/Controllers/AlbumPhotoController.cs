using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class AlbumPhotoController : BaseController
    {
        private ApplicationUserManager _userManager;

        public AlbumPhotoController(IService service) : base(service)
        {
        }

        public AlbumPhotoController(IService service, ApplicationUserManager userManager)
            : base(service)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [Access(Resources.AlbumPhoto, Operations.Read)]
        public async Task<ActionResult> Index(string subzone, string album, string year, string batch, int? page)
        {
            var yr = Service.Year.Get();
            yr.Insert(0, new ServiceYear {Id = -2, Year = "All"});
            ViewBag.year = new SelectList(yr, "Year", "Year");

            var subzones = ListOfThings.ZoneSubZoneList();
            subzones.Insert(0, new SelectListItem {Text = "All", Value = "All"});
            ViewBag.subzone = subzones;

            var returnBatch = batch;
            var returnAlbum = album;

            if (year == "All" || year == "") year = null;
            if (batch == "All" || batch == "") batch = null;
            if (subzone == "All" || subzone == "") subzone = null;
            if (album == "All" || album == "") album = null;

            var albumphotos = await Service.AlbumPhoto.GetAsync();

            var albumpictures = albumphotos.Where(
                x =>
                    (x.Year.Year == year || year == null) &&
                    (x.Batch.BatchName == batch || batch == null) &&
                    (x.Album.SubZone == subzone || subzone == null) &&
                    (x.Album.Name == album || album == null)).ToList().ToPagedList(page ?? 1, 10);

            ViewBag.batch = new SelectList(Service.Batch.Get().Where(x => x.Year.Year == year), "BatchName",
                "BatchName", returnBatch);

            ViewBag.album = new SelectList(Service.Album.Get().Where(x => x.SubZone == subzone), "Name",
                "Name", returnAlbum);
            return View(albumpictures);
        }

        [Access(Resources.AlbumPhoto, Operations.Create)]
        public async Task<ActionResult> Create()
        {
            ViewBag.AlbumId = new SelectList(await Service.Album.GetAsync(), "Id", "Name");
            ViewBag.YearId = new SelectList(await Service.Year.GetAsync(), "Id", "Year");

            return View();
        }

        [Access(Resources.AlbumPhoto, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AlbumPhotos albumPhotos, IEnumerable<PicsDesc> picsDescs)
        {
            var userRoles = UserManager.GetRoles(User.Identity.GetUserId());
            var subzone = await Service.Album.GetAsync(albumPhotos.AlbumId);
            var isAllowed = userRoles.Any(userRole => userRole.Contains(subzone.SubZone));
            if (ModelState.IsValid)
            {
                if (isAllowed || User.IsInRole("Administrator") || User.IsInRole("administrator"))
                {
                    var albumphoto = new AlbumPhoto
                    {
                        AlbumId = albumPhotos.AlbumId,
                        YearId = albumPhotos.YearId,
                        BatchId = albumPhotos.BatchId,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    };

                    foreach (var photo in picsDescs.Where(photo => photo.PictureUpload != null && photo.Description != null && photo.PictureUpload.ContentLength > 0))
                    {
                        albumphoto.ContentType = photo.PictureUpload.ContentType;
                        albumphoto.Photo = ImageConpresser.CompressImage(photo.PictureUpload.InputStream);
                        albumphoto.Description = photo.Description;

                        await Service.AlbumPhoto.AddAsync(albumphoto);
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("",
                    "You are not authorized to add album photos other than those in your sub zone");
            }

            ViewBag.AlbumId = new SelectList(await Service.Album.GetAsync(), "Id", "Name", albumPhotos.AlbumId);
            ViewBag.YearId = new SelectList(await Service.Year.GetAsync(), "Id", "Year", albumPhotos.YearId);
            return View(albumPhotos);
        }

        [Access(Resources.AlbumPhoto, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var albumPhoto = await Service.AlbumPhoto.GetAsync(id);
            if (albumPhoto == null)
            {
                return HttpNotFound();
            }

            var albumphotomodel = new AlbumPhotos
            {
                Id = albumPhoto.Id,
                Photo = albumPhoto.Photo,
                Description = albumPhoto.Description,
                AlbumId = albumPhoto.AlbumId,
                YearId = albumPhoto.YearId,
                BatchId = albumPhoto.BatchId,
                ContentType = albumPhoto.ContentType,
                DateCreated = albumPhoto.DateCreated
            };

            ViewBag.AlbumId = new SelectList(await Service.Album.GetAsync(), "Id", "Name", albumPhoto.AlbumId);
            ViewBag.BatchId = new SelectList(Service.Batch.Get().Where(x => x.YearId == albumPhoto.YearId), "Id",
                "BatchName", albumPhoto.BatchId);
            ViewBag.YearId = new SelectList(await Service.Year.GetAsync(), "Id", "Year", albumPhoto.YearId);
            return View(albumphotomodel);
        }

        [Access(Resources.AlbumPhoto, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AlbumPhotos albumPhotos, HttpPostedFileBase upload)
        {
            var userRoles = UserManager.GetRoles(User.Identity.GetUserId());
            var subzone = await Service.Album.GetAsync(albumPhotos.AlbumId);
            var isAllowed = userRoles.Any(userRole => userRole.Contains(subzone.SubZone));
            if (ModelState.IsValid)
            {
                if (isAllowed || User.IsInRole("Administrator") || User.IsInRole("administrator"))
                {
                    var albumphoto = new AlbumPhoto
                    {
                        AlbumId = albumPhotos.AlbumId,
                        YearId = albumPhotos.YearId,
                        BatchId = albumPhotos.BatchId,
                        Description = albumPhotos.Description,
                        DateModified = DateTime.Now,
                        ContentType = albumPhotos.ContentType,
                        Id = albumPhotos.Id,
                        Photo = albumPhotos.Photo
                    };

                    if (upload != null && upload.ContentLength > 0)
                    {
                        albumphoto.ContentType = upload.ContentType;
                        albumphoto.Photo = ImageConpresser.CompressImage(upload.InputStream);
                    }
                    await Service.AlbumPhoto.UpdateAsync(albumphoto);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("",
                    "You are not authorized to update album photos details other than those in your sub zone");
            }

            ViewBag.AlbumId = new SelectList(await Service.Album.GetAsync(), "Id", "Name", albumPhotos.AlbumId);
            ViewBag.BatchId = new SelectList(Service.Batch.Get().Where(x => x.YearId == albumPhotos.YearId), "Id",
                "BatchName", albumPhotos.BatchId);
            ViewBag.YearId = new SelectList(await Service.Year.GetAsync(), "Id", "Year", albumPhotos.YearId);
            return View(albumPhotos);
        }

        [Access(Resources.AlbumPhoto, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var albumPhoto = await Service.AlbumPhoto.GetAsync(id);
            if (albumPhoto == null)
            {
                return HttpNotFound();
            }
            return View(albumPhoto);
        }

        [Access(Resources.AlbumPhoto, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var albumPhoto = await Service.AlbumPhoto.GetAsync(id);
            await Service.AlbumPhoto.DeleteAsync(albumPhoto);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.AlbumPhoto.Dispose();
                Service.Album.Dispose();
                Service.Batch.Dispose();
                Service.Year.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}