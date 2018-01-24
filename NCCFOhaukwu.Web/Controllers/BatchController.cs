using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class BatchController : BaseController
    {
        public BatchController(IService service) : base(service)
        {
        }

        [Access(Resources.Batch, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var batches = await Service.Batch.GetAsync();
            if (string.IsNullOrEmpty(search))
                return View(batches.OrderBy(x => x.Year.Year).ThenBy(x => x.BatchName).ToPagedList(page ?? 1, 10));
            batches =
                batches.Where(
                    x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return View(batches.OrderBy(x => x.Year.Year).ThenBy(x => x.BatchName).ToPagedList(page ?? 1, 10));
        }

        [Access(Resources.Batch, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var batch = await Service.Batch.GetAsync(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            return View(batch);
        }

        [Access(Resources.Batch, Operations.Create)]
        public ActionResult Create()
        {
            ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year");
            return View();
        }

        [Access(Resources.Batch, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                var batches = await Service.Batch.GetAsync();
                var existingBatch = batches.Count(x => x.YearId == batch.YearId && x.BatchName == batch.BatchName);
                if (existingBatch < 1)
                {
                    batch.BatchName = batch.BatchName.ToUpper();
                    batch.SearchTag = batch.BatchName.ToLower();
                    await Service.Batch.AddAsync(batch);
                }
                else
                {
                    ModelState.AddModelError("", "Batch already exist");
                    ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year", batch.YearId);
                    return View(batch);
                }
                return RedirectToAction("Index");
            }

            ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year", batch.YearId);
            return View(batch);
        }

        [Access(Resources.Batch, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var batch = await Service.Batch.GetAsync(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year", batch.YearId);
            return View(batch);
        }

        [Access(Resources.Batch, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Batch batch)
        {
            if (ModelState.IsValid)
            {
                batch.SearchTag = batch.BatchName.ToLower();
                await Service.Batch.UpdateAsync(batch);
                return RedirectToAction("Index");
            }
            ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year", batch.YearId);
            return View(batch);
        }

        [Access(Resources.Batch, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var batch = await Service.Batch.GetAsync(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            return View(batch);
        }

        [Access(Resources.Batch, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var batch = await Service.Batch.GetAsync(id);
            await Service.Batch.DeleteAsync(batch);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Batch.Dispose();
                Service.Year.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}