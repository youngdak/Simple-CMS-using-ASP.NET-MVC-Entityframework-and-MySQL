using System;
using System.Collections.Generic;
using System.IO;
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
    public class ProjectPictureController : BaseController
    {
        private ApplicationUserManager _userManager;

        public ProjectPictureController(IService service) : base(service)
        {
        }

        public ProjectPictureController(IService service, ApplicationUserManager userManager)
            : base(service)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [Access(Resources.ProjectPicture, Operations.Read)]
        public async Task<ActionResult> Index(string subzone, string project, int? page)
        {
            var subzones = ListOfThings.ZoneSubZoneList();
            subzones.Insert(0, new SelectListItem {Text = "All", Value = "All"});
            ViewBag.subzone = subzones;

            var returnProject = project;

            if (subzone == "All" || subzone == "") subzone = null;
            if (project == "All" || project == "") project = null;

            var projectPictures = await Service.ProjectPicture.GetAsync();
            var projectpics = projectPictures.Where(
                x =>
                    (x.Project.Name == project || project == null) &&
                    (x.Project.SubZone == subzone || subzone == null)).ToList().ToPagedList(page ?? 1, 10);

            ViewBag.project = new SelectList(Service.Project.Get().Where(x => x.SubZone == subzone), "Name",
                "Name", returnProject);

            return View(projectpics);
        }

        [Access(Resources.ProjectPicture, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projectPicture = await Service.ProjectPicture.GetAsync(id);
            if (projectPicture == null)
            {
                return HttpNotFound();
            }
            return View(projectPicture);
        }

        [Access(Resources.ProjectPicture, Operations.Create)]
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(Service.Project.Get(), "Id", "Name");
            return View();
        }

        [Access(Resources.ProjectPicture, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectPicture projectPicture, IEnumerable<PicsDesc> picsDescs)
        {
            var userRoles = UserManager.GetRoles(User.Identity.GetUserId());
            var subzone = await Service.Project.GetAsync(projectPicture.ProjectId);
            var isAllowed = userRoles.Any(userRole => userRole.Contains(subzone.SubZone));
            if (ModelState.IsValid)
            {
                if (isAllowed || User.IsInRole("Administrator") || User.IsInRole("administrator"))
                {
                    projectPicture.DateCreated = DateTime.Now;
                    projectPicture.DateModified = DateTime.Now;
                    foreach (var photo in picsDescs.Where(photo => photo.PictureUpload != null && photo.Description != null && photo.PictureUpload.ContentLength > 0))
                    {
                        projectPicture.ContentType = photo.PictureUpload.ContentType;
                        projectPicture.Photo = ImageConpresser.CompressImage(photo.PictureUpload.InputStream);
                        projectPicture.Description = photo.Description;

                        await Service.ProjectPicture.AddAsync(projectPicture);
                    }

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("",
                    "You are not authorized to add project photos other than those in your sub zone");
            }

            ViewBag.ProjectId = new SelectList(Service.Project.Get(), "Id", "Name", projectPicture.ProjectId);
            return View(projectPicture);
        }

        [Access(Resources.ProjectPicture, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projectPicture = await Service.ProjectPicture.GetAsync(id);
            if (projectPicture == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(Service.Project.Get(), "Id", "Name", projectPicture.ProjectId);
            return View(projectPicture);
        }

        [Access(Resources.ProjectPicture, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProjectPicture projectPicture, HttpPostedFileBase upload)
        {
            var userRoles = UserManager.GetRoles(User.Identity.GetUserId());
            var subzone = await Service.Project.GetAsync(projectPicture.ProjectId);
            var isAllowed = userRoles.Any(userRole => userRole.Contains(subzone.SubZone));
            if (ModelState.IsValid)
            {
                if (isAllowed || User.IsInRole("Administrator") || User.IsInRole("administrator"))
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        projectPicture.ContentType = upload.ContentType;
                        projectPicture.Photo = ImageConpresser.CompressImage(upload.InputStream);
                    }

                    projectPicture.DateModified = DateTime.Now;

                    await Service.ProjectPicture.UpdateAsync(projectPicture);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("",
                    "You are not authorized to update project photo details other than those in your sub zone");
            }
            ViewBag.ProjectId = new SelectList(Service.Project.Get(), "Id", "Name", projectPicture.ProjectId);
            return View(projectPicture);
        }

        [Access(Resources.ProjectPicture, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projectPicture = await Service.ProjectPicture.GetAsync(id);
            if (projectPicture == null)
            {
                return HttpNotFound();
            }
            return View(projectPicture);
        }

        [Access(Resources.ProjectPicture, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var projectPicture = await Service.ProjectPicture.GetAsync(id);
            await Service.ProjectPicture.DeleteAsync(projectPicture);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.ProjectPicture.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}