using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;

namespace NCCFOhaukwu.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IService service)
            : base(service)
        {
        }

       // [OutputCache(Duration = 3600, VaryByParam = "*")]
        public async Task<ActionResult> Index()
        {
            var news = await Service.News.GetAsync();
            var thenews = news.Where(x => x.Display).ToList();

            var articles = await Service.Articles.GetAsync();
            var thearticles = articles.Where(x => x.Display).ToList();

            var events = await Service.Event.GetAsync();
            var theevents = events.Where(x => x.start >= DateTime.Now.Date).Take(4).OrderBy(x => x.start).ToList();

            var sermons = await Service.Sermon.GetAsync();
            var sermon = sermons.FirstOrDefault(x => x.Date >= DateTime.Now.Date);

            var carouselImgs = await Service.Carousel.GetAsync();

            var home = new Home
            {
                Events = theevents,
                News = thenews,
                Carousels = carouselImgs,
                Articles = thearticles,
                Sermon = sermon
            };

            return View(home);
        }

    }
}