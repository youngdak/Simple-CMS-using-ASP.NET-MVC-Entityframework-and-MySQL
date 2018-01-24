using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.BLL.Service;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class SermonController : BaseController
    {
        public SermonController(IService service)
            : base(service)
        {
        }

        [Route("sermons")]
        public async Task<ActionResult> Sermons(int? page)
        {
            var sermons = await Service.Sermon.GetAsync();
            return
                View(
                    sermons.OrderByDescending(x => x.Date)
                        .SkipWhile(x => x.Date > DateTime.Now)
                        .OrderBy(x => x.Date)
                        .ToPagedList(page ?? 1, 5));
        }

        //[OutputCache(Duration = 300, VaryByParam = "*")]
        [Route("sermons/search")]
        public async Task<ActionResult> SearchSermons(string search, int? page)
        {
            var sermons = await Service.Sermon.GetAsync();

            if (string.IsNullOrEmpty(search)) return View(sermons.OrderBy(x => x.Date).ToPagedList(page ?? 1, 5));
            sermons =
                sermons.Where(
                    x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return View(
                    sermons.OrderByDescending(x => x.Date)
                        .SkipWhile(x => x.Date > DateTime.Now)
                        .OrderBy(x => x.Date)
                        .ToPagedList(page ?? 1, 5));
        }

        //[OutputCache(Duration = 300, VaryByParam = "*")]
        //[Route("sermons/{id}/{*title}")]
        //public async Task<ActionResult> Sermon(int id)
        //{
        //    var sermons = await Service.Sermon.GetAsync();
        //    var sermon = sermons.FirstOrDefault(x => x.Id == id);
        //    return View(sermon);
        //}
    }
}