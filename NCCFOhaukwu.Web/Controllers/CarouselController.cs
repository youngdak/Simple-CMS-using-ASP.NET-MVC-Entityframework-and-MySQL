using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;

namespace NCCFOhaukwu.Web.Controllers
{
    public class CarouselController : BaseController
    {
        public CarouselController(IService service)
            : base(service)
        {
        }

        [Access(Resources.Carousel, Operations.Read)]
        public async Task<ActionResult> Index()
        {
            return View(await Service.Carousel.GetAsync());
        }

        [Access(Resources.Carousel, Operations.Create)]
        public ActionResult Create()
        {
            return View();
        }

        [Access(Resources.Carousel, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Carousel carousel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    carousel.ContentType = upload.ContentType;
                    carousel.Img = CarouselImageConpresser.CompressImage(upload.InputStream);
                }
                await Service.Carousel.AddAsync(carousel);
                return RedirectToAction("Index");
            }

            return View(carousel);
        }

        [Access(Resources.Carousel, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var carousel = await Service.Carousel.GetAsync(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }
            return View(carousel);
        }

        [Access(Resources.Carousel, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Carousel carousel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    carousel.ContentType = upload.ContentType;
                    carousel.Img = CarouselImageConpresser.CompressImage(upload.InputStream);
                }

                await Service.Carousel.UpdateAsync(carousel);
                return RedirectToAction("Index");
            }
            return View(carousel);
        }

        [Access(Resources.Carousel, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var carousel = await Service.Carousel.GetAsync(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }
            return View(carousel);
        }

        [Access(Resources.Carousel, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var carousel = await Service.Carousel.GetAsync(id);
            await Service.Carousel.DeleteAsync(carousel);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Carousel.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}