using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.BLL.Service;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class ArticleController : BaseController
    {
        public ArticleController(IService service) : base(service)
        {
        }

        [Route("articles")]
        public async Task<ActionResult> Articles(int? page)
        {
            var articles = await Service.Articles.GetAsync();
            return View(articles.OrderBy(x => x.Display).ThenBy(x => x.DateModified).ToPagedList(page ?? 1, 5));
        }

        //[OutputCache(Duration = 300, VaryByParam = "*")]
        [Route("articles/search")]
        public async Task<ActionResult> SearchArticles(string search, int? page)
        {
            var articles = await Service.Articles.GetAsync();

            if (string.IsNullOrEmpty(search))
                return View(articles.OrderBy(x => x.Display).ThenBy(x => x.DateModified).ToPagedList(page ?? 1, 5));
            articles =
                articles.Where(
                    x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return View(articles.OrderBy(x => x.Display).ThenBy(x => x.DateModified).ToPagedList(page ?? 1, 5));
        }

        //[OutputCache(Duration = 300, VaryByParam = "*")]
        [Route("articles/{id}/{*title}")]
        public async Task<ActionResult> Article(int id)
        {
            var articles = await Service.Articles.GetAsync();
            var article = articles.FirstOrDefault(x => x.Id == id);
            return View(article);
        }
    }
}