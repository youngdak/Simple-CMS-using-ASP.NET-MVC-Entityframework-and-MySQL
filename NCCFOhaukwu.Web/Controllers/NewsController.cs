using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.BLL.Service;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class NewsController : BaseController
    {
        public NewsController(IService service) : base(service)
        {
        }

        //[OutputCache(Duration = 300, VaryByParam = "*")]
        [Route("news")]
        public async Task<ActionResult> News(int? page)
        {
            var news = await Service.News.GetAsync();
            return View(news.OrderBy(x => x.Display).ThenBy(x => x.DateModified).ToPagedList(page ?? 1, 5));
        }

        //[OutputCache(Duration = 300, VaryByParam = "*")]
        [Route("news/search")]
        public async Task<ActionResult> SearchNews(string search, int? page)
        {
            var news = await Service.News.GetAsync();

            if (string.IsNullOrEmpty(search))
                return View(news.OrderBy(x => x.Display).ThenBy(x => x.DateModified).ToPagedList(page ?? 1, 5));
            news =
                news.Where(
                    x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return View(news.OrderBy(x => x.Display).ThenBy(x => x.DateModified).ToPagedList(page ?? 1, 5));
        }

        //[OutputCache(Duration = 300, VaryByParam = "*")]
        [Route("news/{id}/{*title}")]
        public async Task<ActionResult> SingleNews(int id)
        {
            var news = await Service.News.GetAsync();
            var thenews = news.FirstOrDefault(x => x.Id == id);
            return View(thenews);
        }
    }
}