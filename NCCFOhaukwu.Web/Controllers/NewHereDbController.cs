using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;

namespace NCCFOhaukwu.Web.Controllers
{
    public class NewHereDbController : BaseController
    {
        public NewHereDbController(IService service)
            : base(service)
        {
        }

        [Access(Resources.NewHere, Operations.Read)]
        public async Task<ActionResult> Index()
        {
            return View(await Service.NewHere.GetAsync());
        }

        [Access(Resources.NewHere, Operations.Create)]
        public ActionResult Create()
        {
            return View(new NewHereModel());
        }

        [Access(Resources.NewHere, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewHereModel newHereModel)
        {
            var whoweares = await Service.NewHere.GetAsync();
            if (whoweares.Count > 0)
            {
                ModelState.AddModelError("", "Cannot have more than one new here data");
                return View(newHereModel);
            }

            if (ModelState.IsValid)
            {
                var newhere = new NewHere
                {
                    NewToNccf = newHereModel.NewToNccf,
                    ServiceTimesandLocations = newHereModel.ServiceTimesandLocations,
                    WhatToExpect = newHereModel.WhatToExpect,
                    FamilySong = newHereModel.FamilySong
                };

                await Service.NewHere.AddAsync(newhere);
                return RedirectToAction("Index");
            }

            return View(newHereModel);
        }

        [Access(Resources.NewHere, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var newHere = await Service.NewHere.GetAsync(id);
            if (newHere == null)
            {
                return HttpNotFound();
            }

            var newHereModel = new NewHereModel
            {
                Id = newHere.Id,
                WhatToExpect = newHere.WhatToExpect,
                ServiceTimesandLocations = newHere.ServiceTimesandLocations,
                NewToNccf = newHere.NewToNccf,
                FamilySong = newHere.FamilySong
            };

            return View(newHereModel);
        }

        [Access(Resources.NewHere, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(NewHereModel newHereModel)
        {
            if (ModelState.IsValid)
            {
                var newhere = new NewHere
                {
                    Id = newHereModel.Id,
                    NewToNccf = newHereModel.NewToNccf,
                    ServiceTimesandLocations = newHereModel.ServiceTimesandLocations,
                    WhatToExpect = newHereModel.WhatToExpect,
                    FamilySong = newHereModel.FamilySong
                };

                await Service.NewHere.UpdateAsync(newhere);
                return RedirectToAction("Index");
            }
            return View(newHereModel);
        }

        [Access(Resources.NewHere, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var newHere = await Service.NewHere.GetAsync(id);
            if (newHere == null)
            {
                return HttpNotFound();
            }
            return View(newHere);
        }

        [Access(Resources.NewHere, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var newHere = await Service.NewHere.GetAsync(id);
            await Service.NewHere.DeleteAsync(newHere);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.NewHere.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}