using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;

namespace NCCFOhaukwu.Web.Controllers
{
    public class WhoWeAreDbController : BaseController
    {
        public WhoWeAreDbController(IService service) : base(service)
        {
        }

        [Access(Resources.WhoWeAre, Operations.Read)]
        public async Task<ActionResult> Index()
        {
            return View(await Service.WhoWeAre.GetAsync());
        }

        [Access(Resources.WhoWeAre, Operations.Create)]
        public ActionResult Create()
        {
            return View(new WhoWeAreModel());
        }

        [Access(Resources.WhoWeAre, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WhoWeAreModel whoWeAreModel)
        {
            var whoweares = await Service.WhoWeAre.GetAsync();
            if (whoweares.Count > 0)
            {
                ModelState.AddModelError("", "Cannot have more than one who we are data");
                return View(whoWeAreModel);
            }
            if (ModelState.IsValid)
            {
                var whoweare = new WhoWeAre
                {
                    Whoweare = whoWeAreModel.Whoweare,
                    OurBeliefs = whoWeAreModel.OurBeliefs,
                    OurHistory = whoWeAreModel.OurHistory,
                    OurMission = whoWeAreModel.OurMission
                };

                await Service.WhoWeAre.AddAsync(whoweare);
                return RedirectToAction("Index");
            }

            return View(whoWeAreModel);
        }

        [Access(Resources.WhoWeAre, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var whoWeAre = await Service.WhoWeAre.GetAsync(id);

            if (whoWeAre == null)
            {
                return HttpNotFound();
            }
            var whoweareModel = new WhoWeAreModel
            {
                Id = whoWeAre.Id,
                Whoweare = whoWeAre.Whoweare,
                OurMission = whoWeAre.OurMission,
                OurHistory = whoWeAre.OurHistory,
                OurBeliefs = whoWeAre.OurBeliefs
            };
            return View(whoweareModel);
        }

        [Access(Resources.WhoWeAre, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WhoWeAreModel whoWeAreModel)
        {
            if (ModelState.IsValid)
            {
                var whoweare = new WhoWeAre
                {
                    Id = whoWeAreModel.Id,
                    Whoweare = whoWeAreModel.Whoweare,
                    OurBeliefs = whoWeAreModel.OurBeliefs,
                    OurHistory = whoWeAreModel.OurHistory,
                    OurMission = whoWeAreModel.OurMission
                };
                await Service.WhoWeAre.UpdateAsync(whoweare);
                return RedirectToAction("Index");
            }
            return View(whoWeAreModel);
        }

        [Access(Resources.WhoWeAre, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var whoWeAre = await Service.WhoWeAre.GetAsync(id);
            if (whoWeAre == null)
            {
                return HttpNotFound();
            }
            return View(whoWeAre);
        }

        [Access(Resources.WhoWeAre, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var whoWeAre = await Service.WhoWeAre.GetAsync(id);
            await Service.WhoWeAre.DeleteAsync(whoWeAre);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.WhoWeAre.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}