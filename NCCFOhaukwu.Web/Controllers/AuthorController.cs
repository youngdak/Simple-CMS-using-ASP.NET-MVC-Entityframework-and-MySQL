using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.BLL.Service;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IService _service;

        public AuthorController()
        {
            _service = new Service();
        }

        [Route("author/news/{username}")]
        public async Task<ActionResult> AuthorNews(string username, int? page)
        {
            var news = await _service.News.GetAsync();
            var authorsnews = news.Where(x => x.Username.ToLower() == username.Replace("-", " ").ToLower());
            var firstOrDefault = news.FirstOrDefault(x => x.Username.ToLower() == username.Replace("-", " ").ToLower());
            if (firstOrDefault != null)
                ViewBag.authorname =
                    firstOrDefault.Author.FullName;
            return View(authorsnews.ToPagedList(page ?? 1, 5));
        }

        [Route("author/articles/{username}")]
        public async Task<ActionResult> AuthorArticles(string username, int? page)
        {
            var articles = await _service.Articles.GetAsync();
            var article = articles.Where(x => x.Username.ToLower() == username.Replace("-", " ").ToLower());
            var firstOrDefault = articles.FirstOrDefault(x => x.Username.ToLower() == username.Replace("-", " ").ToLower());
            if (firstOrDefault != null)
                ViewBag.authorname =
                    firstOrDefault.Author.FullName;
            return View(article.ToPagedList(page ?? 1, 5));
        }
    }
}