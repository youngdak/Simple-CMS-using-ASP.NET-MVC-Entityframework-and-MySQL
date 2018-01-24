using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class NewsDbController : BaseController
    {
        public NewsDbController(IService service) : base(service)
        {
        }

        [Access(Resources.News, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var news = await Service.News.GetAsync();
            if (string.IsNullOrEmpty(search))
                return
                    View(news.OrderBy(x => x.Display)
                        .ThenBy(x => x.Title)
                        .ThenBy(x => x.DateModified)
                        .ToPagedList(page ?? 1, 10));
            news =
                news.Where(
                    x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return
                View(news.OrderBy(x => x.Display)
                    .ThenBy(x => x.Title)
                    .ThenBy(x => x.DateModified)
                    .ToPagedList(page ?? 1, 10));
        }

        [Access(Resources.News, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = await Service.News.GetAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        [Access(Resources.News, Operations.Create)]
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(Service.CorpMember.Get(), "Id", "FullName");
            return View(new TheNews());
        }

        [Access(Resources.News, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TheNews thenews, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                var news = new News
                {
                    TheNews = thenews.News,
                    AuthorId = thenews.AuthorId,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Title = thenews.Title,
                    Display = thenews.Display,
                    Username =
                        Service.CorpMember.Get(thenews.AuthorId).FullName.ToLower().Replace(" ", "") + "nccfohaukwu"
                };
                news.SearchTag = news.Title.ToLower() + news.Username.ToLower() + Service.CorpMember.Get(thenews.AuthorId).FullName.ToLower();

                if (upload != null && upload.ContentLength > 0)
                {
                    news.ContentType = upload.ContentType;
                    news.Photo = ImageConpresser.CompressImage(upload.InputStream);
                }

                await Service.News.AddAsync(news);
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(await Service.CorpMember.GetAsync(), "Id", "FullName", thenews.AuthorId);
            return View(thenews);
        }

        [Access(Resources.News, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = await Service.News.GetAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }

            var thenews = new TheNews
            {
                Id = news.Id,
                AuthorId = news.AuthorId,
                ContentType = news.ContentType,
                DateCreated = news.DateCreated,
                DateModified = news.DateModified,
                Display = news.Display,
                Photo = news.Photo,
                News = news.TheNews,
                Title = news.Title,
                Username = news.Username
            };

            ViewBag.AuthorId = new SelectList(await Service.CorpMember.GetAsync(), "Id", "FullName", news.AuthorId);
            return View(thenews);
        }

        [Access(Resources.News, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TheNews thenews, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                var news = new News
                {
                    Id = thenews.Id,
                    TheNews = thenews.News,
                    AuthorId = thenews.AuthorId,
                    DateCreated = thenews.DateCreated,
                    DateModified = DateTime.Now,
                    Title = thenews.Title,
                    Username = thenews.Username,
                    Display = thenews.Display,
                    ContentType = thenews.ContentType,
                    Photo = thenews.Photo
                };
                news.SearchTag = news.Title.ToLower() + news.Username.ToLower() + Service.CorpMember.Get(thenews.AuthorId).FullName.ToLower();

                if (upload != null && upload.ContentLength > 0)
                {
                    news.ContentType = upload.ContentType;
                    news.Photo = ImageConpresser.CompressImage(upload.InputStream);
                }

                await Service.News.UpdateAsync(news);
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(await Service.CorpMember.GetAsync(), "Id", "FullName", thenews.AuthorId);
            return View(thenews);
        }

        [Access(Resources.News, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = await Service.News.GetAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        [Access(Resources.News, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var news = await Service.News.GetAsync(id);
            await Service.News.DeleteAsync(news);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.News.Dispose();
                Service.CorpMember.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}