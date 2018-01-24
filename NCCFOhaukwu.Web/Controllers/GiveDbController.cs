using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;

namespace NCCFOhaukwu.Web.Controllers
{
    public class GiveDbController : BaseController
    {
        public GiveDbController(IService service)
            : base(service)
        {
        }

        [Access(Resources.Give, Operations.Read)]
        public async Task<ActionResult> Index()
        {
            return View(await Service.Give.GetAsync());
        }

        [Access(Resources.Give, Operations.Create)]
        public async Task<ActionResult> Create()
        {
            return View(new GiveModel());
        }

        [Access(Resources.Give, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GiveModel giveModel)
        {
            var gives = await Service.Give.GetAsync();
            if (gives.Count > 0)
            {
                ModelState.AddModelError("", "Cannot have more than one give data");
                return View(giveModel);
            }
            if (ModelState.IsValid)
            {
                var give = new Give
                {
                    Donate = giveModel.Donate
                };
                await Service.Give.AddAsync(give);
                return RedirectToAction("Index");
            }

            return View(giveModel);
        }

        [Access(Resources.Give, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var give = await Service.Give.GetAsync(id);
            if (give == null)
            {
                return HttpNotFound();
            }
            var giveModel = new GiveModel
            {
                Id = give.Id,
                Donate = give.Donate
            };
            return View(giveModel);
        }

        [Access(Resources.Give, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GiveModel giveModel)
        {
            if (ModelState.IsValid)
            {
                var give = new Give
                {
                    Id = giveModel.Id,
                    Donate = giveModel.Donate
                };
                await Service.Give.UpdateAsync(give);
                return RedirectToAction("Index");
            }
            return View(giveModel);
        }

        [Access(Resources.Give, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var give = await Service.Give.GetAsync(id);
            if (give == null)
            {
                return HttpNotFound();
            }
            return View(give);
        }

        [Access(Resources.Give, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var give = await Service.Give.GetAsync(id);
            await Service.Give.DeleteAsync(give);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Give.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}