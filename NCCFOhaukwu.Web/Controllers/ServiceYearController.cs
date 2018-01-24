using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class ServiceYearController : BaseController
    {
        public ServiceYearController(IService service) : base(service)
        {
        }

        [Access(Resources.ServiceYear, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var serviceyears = await Service.Year.GetAsync();
            if (string.IsNullOrEmpty(search)) return View(serviceyears.OrderBy(x => x.Year).ToPagedList(page ?? 1, 10));
            serviceyears =
                serviceyears.Where(
                    x =>
                        x.Year.ToLower().Contains(search.ToLower())).ToList();

            ViewBag.search = search;
            return View(serviceyears.OrderBy(x => x.Year).ToPagedList(page ?? 1, 10));
        }

        [Access(Resources.ServiceYear, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var serviceYear = await Service.Year.GetAsync(id);
            if (serviceYear == null)
            {
                return HttpNotFound();
            }
            return View(serviceYear);
        }

        [Access(Resources.ServiceYear, Operations.Create)]
        public ActionResult Create()
        {
            return View();
        }

        [Access(Resources.ServiceYear, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ServiceYear serviceYear)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await Service.Year.AddAsync(serviceYear);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                    {
                        ModelState.AddModelError("", serviceYear.Year + " already exist");
                    }
                    return View(serviceYear);
                }

                return RedirectToAction("Index");
            }

            return View(serviceYear);
        }

        [Access(Resources.ServiceYear, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var serviceYear = await Service.Year.GetAsync(id);
            if (serviceYear == null)
            {
                return HttpNotFound();
            }
            return View(serviceYear);
        }

        [Access(Resources.ServiceYear, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ServiceYear serviceYear)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await Service.Year.UpdateAsync(serviceYear);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                    {
                        ModelState.AddModelError("", serviceYear.Year + " already exist");
                    }
                    return View(serviceYear);
                }
                return RedirectToAction("Index");
            }
            return View(serviceYear);
        }

        [Access(Resources.ServiceYear, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var serviceYear = await Service.Year.GetAsync(id);
            if (serviceYear == null)
            {
                return HttpNotFound();
            }
            return View(serviceYear);
        }

        [Access(Resources.ServiceYear, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var serviceYear = await Service.Year.GetAsync(id);
            await Service.Year.DeleteAsync(serviceYear);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Year.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}