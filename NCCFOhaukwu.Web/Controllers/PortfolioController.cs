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
    public class PortfolioController : BaseController
    {
        public PortfolioController(IService service) : base(service)
        {
        }

        [Access(Resources.Portfolio, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var portfolios = await Service.Portfolio.GetAsync();

            if (string.IsNullOrEmpty(search))
                return
                    View(portfolios.OrderBy(x => x.Position).ToPagedList(page ?? 1, 10));
            portfolios =
                portfolios.Where(
                    x =>
                        x.Position.ToLower().Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return
                View(portfolios.OrderBy(x => x.Position).ToPagedList(page ?? 1, 10));
        }

        [Access(Resources.Portfolio, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var portfolio = await Service.Portfolio.GetAsync(id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        [Access(Resources.Portfolio, Operations.Create)]
        public ActionResult Create()
        {
            return View();
        }

        [Access(Resources.Portfolio, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await Service.Portfolio.AddAsync(portfolio);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                    {
                        ModelState.AddModelError("", portfolio.Position + " already exist");
                    }

                    return View(portfolio);
                }

                return RedirectToAction("Index");
            }

            return View(portfolio);
        }

        [Access(Resources.Portfolio, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var portfolio = await Service.Portfolio.GetAsync(id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        [Access(Resources.Portfolio, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await Service.Portfolio.UpdateAsync(portfolio);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                    {
                        ModelState.AddModelError("", portfolio.Position + " already exist");
                    }

                    return View(portfolio);
                }
                return RedirectToAction("Index");
            }
            return View(portfolio);
        }

        [Access(Resources.Portfolio, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var portfolio = await Service.Portfolio.GetAsync(id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        [Access(Resources.Portfolio, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var portfolio = await Service.Portfolio.GetAsync(id);
            await Service.Portfolio.DeleteAsync(portfolio);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Portfolio.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}