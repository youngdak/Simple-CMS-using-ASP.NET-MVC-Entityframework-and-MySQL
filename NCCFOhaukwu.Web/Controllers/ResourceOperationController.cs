using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL;
using Microsoft.AspNet.Identity.Owin;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class ResourceOperationController : BaseController
    {
        private ApplicationRoleManager _roleManager;

        public ResourceOperationController(ApplicationRoleManager roleManager, IService service)
            : base(service)
        {
            RoleManager = roleManager;
        }

        public ResourceOperationController(IService service)
            : base(service)
        {
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }

        [Access(Resources.ResourceOperation, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var o2r = await Service.OperationToRoles.GetAsync();
            if (string.IsNullOrEmpty(search))
                return View(o2r.OrderBy(x => x.RoleName).ThenBy(x => x.ResourceName).ToPagedList(page ?? 1, 10));
            o2r =
                o2r.Where(
                    x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return View(o2r.OrderBy(x => x.RoleName).ThenBy(x => x.ResourceName).ToPagedList(page ?? 1, 10));
        }

        [Access(Resources.ResourceOperation, Operations.Create)]
        public async Task<ActionResult> Create()
        {
            ViewBag.Operations = new SelectList(ListOfThings.OperationsList(), "Value", "Text");
            ViewBag.Roles = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View(new MapOperationsToRoles());
        }

        [Access(Resources.ResourceOperation, Operations.Create)]
        [HttpPost]
        public async Task<ActionResult> Create(MapOperationsToRoles operationsToRoles,
            params string[] selectedOperations)
        {
            if (ModelState.IsValid)
            {
                var o2r = new OperationToRoles
                {
                    Id = operationsToRoles.Id,
                    ResourceName = operationsToRoles.ResourceName,
                    RoleName = operationsToRoles.RoleName,
                    Operations = OperationService.Op(selectedOperations),
                    ResourceId = ResourceService.ResourceId(operationsToRoles.ResourceName),
                    SearchTag = operationsToRoles.RoleName.ToLower() + operationsToRoles.ResourceName.ToLower()
                };
                await Service.OperationToRoles.AddAsync(o2r);
            }
            else
            {
                ViewBag.Operations = new SelectList(ListOfThings.OperationsList(), "Value", "Text");
                ViewBag.Roles = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                return View();
            }

            return RedirectToAction("Index");
        }

        [Access(Resources.ResourceOperation, Operations.Update)]
        public async Task<ActionResult> Edit(int id)
        {
            var o2r = await Service.OperationToRoles.GetAsync(id);

            ViewBag.Roles = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            ViewBag.Resources = new SelectList(ListOfThings.ResourcesList(), "Value", "Text");
            return View(new MapOperationsToRoles
            {
                ResourceName = o2r.ResourceName,
                RoleName = o2r.RoleName,
                OperationList = ListOfThings.OperationsList().Select(x => new SelectListItem
                {
                    Selected = o2r.Operations.HasFlag(OperationService.Op(x.Value)),
                    Text = x.Text,
                    Value = x.Value
                })
            });
        }

        [Access(Resources.ResourceOperation, Operations.Update)]
        [HttpPost]
        public async Task<ActionResult> Edit(MapOperationsToRoles operationsToRoles, params string[] selectedOperations)
        {
            if (ModelState.IsValid)
            {
                var o2r = new OperationToRoles
                {
                    Id = operationsToRoles.Id,
                    ResourceName = operationsToRoles.ResourceName,
                    RoleName = operationsToRoles.RoleName,
                    Operations = OperationService.Op(selectedOperations),
                    ResourceId = ResourceService.ResourceId(operationsToRoles.ResourceName),
                    SearchTag = operationsToRoles.RoleName.ToLower() + operationsToRoles.ResourceName.ToLower()
                };

                await Service.OperationToRoles.UpdateAsync(o2r);
            }
            else
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        [Access(Resources.ResourceOperation, Operations.Delete)]
        public async Task<ActionResult> Delete(int id)
        {
            var o2r = await Service.OperationToRoles.GetAsync(id);
            if (o2r == null)
            {
                return HttpNotFound();
            }
            return View(o2r);
        }

        [Access(Resources.ResourceOperation, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var o2r = await Service.OperationToRoles.GetAsync(id);

            await Service.OperationToRoles.DeleteAsync(o2r);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.OperationToRoles.Dispose();
                RoleManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}