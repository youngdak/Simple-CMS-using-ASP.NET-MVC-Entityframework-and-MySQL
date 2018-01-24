using System;
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
    public class AlbumController : BaseController
    {
        private ApplicationUserManager _userManager;

        public AlbumController(IService service)
            : base(service)
        {
        }

        public AlbumController(IService service, ApplicationUserManager userManager)
            : base(service)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [Access(Resources.Album, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var albums = await Service.Album.GetAsync();
            if (string.IsNullOrEmpty(search)) return View(albums.ToPagedList(page ?? 1, 10));

            albums = albums.Where(x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return View(albums.ToPagedList(page ?? 1, 10));
        }

        [Access(Resources.Album, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var album = await Service.Album.GetAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        [Access(Resources.Album, Operations.Create)]
        public ActionResult Create()
        {
            return View(new Albums());
        }

        [Access(Resources.Album, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Albums albums)
        {
            var userRoles = UserManager.GetRoles(User.Identity.GetUserId());
            var isAllowed =
                userRoles.Any(
                    userRole =>
                        userRole.ToLower().Contains(albums.SubZone.ToLower()) || userRole.ToLower().Contains("admin"));
            if (ModelState.IsValid)
            {
                if (isAllowed)
                {
                    var album = new Album
                    {
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        Name = albums.Name,
                        SubZone = albums.SubZone,
                        SearchTag = albums.Name.ToLower() + albums.SubZone.ToLower()
                    };

                    try
                    {
                        await Service.Album.AddAsync(album);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                        {
                            ModelState.AddModelError("", album.Name + " already exist");
                        }
                        return View(albums);
                    }

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "You are not authorized to add album other than those in your sub zone");
            }

            return View(albums);
        }

        [Access(Resources.Album, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var album = await Service.Album.GetAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            var albumModel = new Albums
            {
                Id = album.Id,
                DateCreated = album.DateCreated,
                Name = album.Name,
                SubZone = album.SubZone
            };

            return View(albumModel);
        }

        [Access(Resources.Album, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Albums albums)
        {
            var userRoles = UserManager.GetRoles(User.Identity.GetUserId());
            var isAllowed = userRoles.Any(userRole => userRole.Contains(albums.SubZone));
            if (ModelState.IsValid)
            {
                if (isAllowed || User.IsInRole("Administrator") || User.IsInRole("administrator"))
                {
                    var album = new Album
                    {
                        Id = albums.Id,
                        DateCreated = albums.DateCreated,
                        DateModified = DateTime.Now,
                        Name = albums.Name,
                        SubZone = albums.SubZone,
                        SearchTag = albums.Name.ToLower() + albums.SubZone.ToLower()
                    };

                    try
                    {
                        await Service.Album.UpdateAsync(album);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                        {
                            ModelState.AddModelError("", album.Name + " already exist");
                        }
                        return View(albums);
                    }

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("",
                    "You are not authorized to update album details other than those in your sub zone");
            }

            return View(albums);
        }

        [Access(Resources.Album, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var album = await Service.Album.GetAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        [Access(Resources.Album, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var album = await Service.Album.GetAsync(id);
            Service.Album.Delete(album);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Album.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}