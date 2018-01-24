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
    public class ArticleDbController : BaseController
    {
        public ArticleDbController(IService service)
            : base(service)
        {
        }

        [Access(Resources.Article, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var articles = await Service.Articles.GetAsync();
            if (string.IsNullOrEmpty(search)) return View(articles.ToPagedList(page ?? 1, 10));
            articles =
                articles.Where(
                    x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return View(articles.ToPagedList(page ?? 1, 10));
        }
        
        [Access(Resources.Article, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var articles = await Service.Articles.GetAsync(id);
            if (articles == null)
            {
                return HttpNotFound();
            }
            return View(articles);
        }
        
        [Access(Resources.Article, Operations.Create)]
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(Service.CorpMember.Get(), "Id", "FullName");
            return View(new ArticlesModel());
        }
        
        [Access(Resources.Article, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ArticlesModel articles)
        {
            if (ModelState.IsValid)
            {
                var article = new Articles
                {
                    Article = articles.Article,
                    Title = articles.Title,
                    Display = articles.Display,
                    AuthorId = articles.AuthorId,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Username = Service.CorpMember.Get(articles.AuthorId).FullName.ToLower().Replace(" ", "") + "nccfohaukwu",
                    SearchTag = articles.Title.ToLower() + articles.Article.ToLower()
                };
                await Service.Articles.AddAsync(article);
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(await Service.CorpMember.GetAsync(), "Id", "FullName", articles.AuthorId);
            return View(articles);
        }
        
        [Access(Resources.Article, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var articles = await Service.Articles.GetAsync(id);
            if (articles == null)
            {
                return HttpNotFound();
            }

            var articleModel = new ArticlesModel
            {
                Id = articles.Id,
                Display = articles.Display,
                Title = articles.Title,
                Article = articles.Article,
                DateCreated = articles.DateCreated,
                DateModified = articles.DateModified,
                Username = articles.Username,
                AuthorId = articles.AuthorId
            };

            ViewBag.AuthorId = new SelectList(await Service.CorpMember.GetAsync(), "Id", "FullName", articles.AuthorId);
            return View(articleModel);
        }
        
        [Access(Resources.Article, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ArticlesModel articles)
        {
            if (ModelState.IsValid)
            {
                var article = new Articles
                {
                    Id = articles.Id,
                    Article = articles.Article,
                    Title = articles.Title,
                    Display = articles.Display,
                    AuthorId = articles.AuthorId,
                    DateCreated = articles.DateCreated,
                    DateModified = DateTime.Now,
                    Username = articles.Username,
                    SearchTag = articles.Title.ToLower() + articles.Article.ToLower()
                };

                await Service.Articles.UpdateAsync(article);
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(await Service.CorpMember.GetAsync(), "Id", "FullName", articles.AuthorId);
            return View(articles);
        }
        
        [Access(Resources.Article, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var articles = await Service.Articles.GetAsync(id);
            if (articles == null)
            {
                return HttpNotFound();
            }
            return View(articles);
        }
        
        [Access(Resources.Article, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var articles = await Service.Articles.GetAsync(id);
            await Service.Articles.DeleteAsync(articles);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Articles.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}