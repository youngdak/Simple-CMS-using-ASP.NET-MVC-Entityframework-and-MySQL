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
    public class ProjectDbController : BaseController
    {
        private ApplicationUserManager _userManager;

        public ProjectDbController(IService service) : base(service)
        {
        }

        public ProjectDbController(IService service, ApplicationUserManager userManager)
            : base(service)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [Access(Resources.Project, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var projects = await Service.Project.GetAsync();

            if (string.IsNullOrEmpty(search))
                return
                    View(projects.OrderBy(x => x.Name).ThenBy(x => x.SubZone).ToPagedList(page ?? 1, 10));
            projects =
                projects.Where(
                    x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return
                View(projects.OrderBy(x => x.Name).ThenBy(x => x.SubZone).ToPagedList(page ?? 1, 10));
        }

        [Access(Resources.Project, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = await Service.Project.GetAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [Access(Resources.Project, Operations.Create)]
        public ActionResult Create()
        {
            return View(new Projects());
        }

        [Access(Resources.Project, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Projects projectModel)
        {
            var userRoles = UserManager.GetRoles(User.Identity.GetUserId());
            var isAllowed = userRoles.Any(userRole => userRole.Contains(projectModel.SubZone));
            if (ModelState.IsValid)
            {
                if (isAllowed || User.IsInRole("Administrator") || User.IsInRole("administrator"))
                {
                    var project = new Project
                    {
                        Name = projectModel.Name,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        Milestone = projectModel.Milestone,
                        SubZone = projectModel.SubZone,
                        SearchTag = projectModel.Name.ToLower() + projectModel.SubZone.ToLower()
                    };

                    try
                    {
                        await Service.Project.AddAsync(project);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                        {
                            ModelState.AddModelError("", project.Name + " already exist");
                        }
                        return View(projectModel);
                    }

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "You are not authorized to add project other than those in your sub zone");
            }

            return View(projectModel);
        }

        [Access(Resources.Project, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = await Service.Project.GetAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var projectmodel = new Projects
            {
                Id = project.Id,
                DateCreated = project.DateCreated,
                DateModified = project.DateModified,
                Milestone = project.Milestone,
                Name = project.Name,
                SubZone = project.SubZone
            };

            return View(projectmodel);
        }

        [Access(Resources.Project, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Projects projectModel)
        {
            var userRoles = UserManager.GetRoles(User.Identity.GetUserId());
            var isAllowed = userRoles.Any(userRole => userRole.Contains(projectModel.SubZone));
            if (ModelState.IsValid)
            {
                if (isAllowed || User.IsInRole("Administrator") || User.IsInRole("administrator"))
                {
                    var project = new Project
                    {
                        Id = projectModel.Id,
                        Name = projectModel.Name,
                        DateCreated = projectModel.DateCreated,
                        DateModified = DateTime.Now,
                        Milestone = projectModel.Milestone,
                        SubZone = projectModel.SubZone,
                        SearchTag = projectModel.Name.ToLower() + projectModel.SubZone.ToLower()
                    };

                    try
                    {
                        await Service.Project.UpdateAsync(project);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                        {
                            ModelState.AddModelError("", project.Name + " already exist");
                        }
                        return View(projectModel);
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("",
                    "You are not authorized to update project details other than those in your sub zone");
            }

            return View(projectModel);
        }

        [Access(Resources.Project, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = await Service.Project.GetAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [Access(Resources.Project, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var project = await Service.Project.GetAsync(id);
            await Service.Project.DeleteAsync(project);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Project.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}