using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class SermonDbController : BaseController
    {
        public SermonDbController(IService service)
            : base(service)
        {
        }

        [Access(Resources.Sermon, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var sermons = await Service.Sermon.GetAsync();
            if (string.IsNullOrEmpty(search)) return View(sermons.ToPagedList(page ?? 1, 10));
            sermons =
                sermons.Where(
                    x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return View(sermons.ToPagedList(page ?? 1, 10));
        }

        [Access(Resources.Sermon, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sermon = await Service.Sermon.GetAsync(id);
            if (sermon == null)
            {
                return HttpNotFound();
            }
            return View(sermon);
        }

        [Access(Resources.Sermon, Operations.Create)]
        public ActionResult Create()
        {
            return View();
        }

        [Access(Resources.Sermon, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SermonModel sermon)
        {
            if (ModelState.IsValid)
            {
                var sermons = new Sermon
                {
                    Date = sermon.Date,
                    Message = sermon.Message,
                    Scriptures = sermon.Scriptures,
                    Title = sermon.Title,
                    SearchTag = sermon.Scriptures.ToLower() + sermon.Title.ToLower() + sermon.Date.ToString("dd/MM/yyyy")
                };

                try
                {
                    await Service.Sermon.AddAsync(sermons);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                    {
                        ModelState.AddModelError("", "This date has sermon already");
                    }
                    return View(sermon);
                }

                return RedirectToAction("Index");
            }

            return View(sermon);
        }

        [Access(Resources.Sermon, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sermon = await Service.Sermon.GetAsync(id);
            if (sermon == null)
            {
                return HttpNotFound();
            }

            var sermonModel = new SermonModel
            {
                Id = sermon.Id,
                Message = sermon.Message,
                Scriptures = sermon.Scriptures,
                Title = sermon.Title,
                Date = sermon.Date
            };


            return View(sermonModel);
        }

        [Access(Resources.Sermon, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SermonModel sermon)
        {
            if (ModelState.IsValid)
            {
                var sermons = new Sermon
                {
                    Id = sermon.Id,
                    Date = sermon.Date,
                    Message = sermon.Message,
                    Scriptures = sermon.Scriptures,
                    Title = sermon.Title,
                    SearchTag = sermon.Scriptures.ToLower() + sermon.Title.ToLower() + sermon.Date.ToString("dd/MM/yyyy")
                };

                try
                {
                    await Service.Sermon.UpdateAsync(sermons);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                    {
                        ModelState.AddModelError("", sermon.Title + " already exist");
                    }
                    return View(sermon);
                }
                return RedirectToAction("Index");
            }
            return View(sermon);
        }

        [Access(Resources.Sermon, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sermon = await Service.Sermon.GetAsync(id);
            if (sermon == null)
            {
                return HttpNotFound();
            }
            return View(sermon);
        }

        [Access(Resources.Sermon, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var sermon = await Service.Sermon.GetAsync(id);
            await Service.Sermon.DeleteAsync(sermon);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Sermon.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}