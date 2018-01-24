using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    //[OutputCache(Duration = 60, VaryByParam = "*")]
    public class WhoWeAreController : BaseController
    {
        public WhoWeAreController(IService service) : base(service)
        {
        }

        //[OutputCache(Duration = 3600, VaryByParam = "*")]
        public async Task<ActionResult> Index()
        {
            var whoweare = await Service.WhoWeAre.GetAsync();
            return View(whoweare.FirstOrDefault());
        }

        public ActionResult CheckNewsDisplay()
        {
            var display = Service.News.Get().Count(x => x.Display);
            return Json(display, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckArticleDisplay()
        {
            var display = Service.Articles.Get().Count(x => x.Display);
            return Json(display, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillBatch(int? id)
        {
            var batchs = Service.Batch.Get().Where(x => x.YearId == id);
            return Json(batchs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Batch(string year)
        {
            var batchs = Service.Batch.Get().Where(x => x.Year.Year == year);
            return Json(batchs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Album(string subzone)
        {
            var subzones = Service.Album.Get().Where(x => x.SubZone == subzone);
            return Json(subzones, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProjectName(string subzone)
        {
            var projectnames = Service.Project.Get().Where(x => x.SubZone == subzone);
            return Json(projectnames, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> JesusCorpers(string year, string batch, string position, string search,
            int? page, string ppatown = "ezzangbo")
        {
            var yr = Service.Year.Get().OrderByDescending(x => x.Year).Take(2).OrderBy(x => x.Year).ToList();
            yr.Insert(0, new ServiceYear {Id = -2, Year = "All"});
            ViewBag.year = new SelectList(yr, "Year", "Year");

            var portfolio = Service.Portfolio.Get();
            portfolio.Insert(0, new Portfolio {Id = 0, Position = "All"});
            portfolio.Insert(1, new Portfolio {Id = -1, Position = "Excos"});
            ViewBag.position = new SelectList(portfolio, "Position", "Position");

            var returnBatch = batch;

            var pos = position;

            if (year == "All" || year == "") year = null;
            if (batch == "All" || batch == "") batch = null;
            if (position == "All" || position == "Excos" || position == "" || position == null) position = "";

            IEnumerable<CorpMember> jesuscorpers;

            var corpmembers = await Service.CorpMember.GetAsync();

            if (pos == "Excos")
                jesuscorpers =
                    corpmembers.Where(
                        x =>
                            (x.ServiceYear.Year == year || year == null) &&
                            (x.Batch.BatchName == batch || batch == null) && x.IsLeader && x.IsGeneral == false).ToList();
            else
                jesuscorpers =
                    corpmembers.Where(
                        x =>
                            (x.ServiceYear.Year == year || year == null) &&
                            (x.Batch.BatchName == batch || batch == null) &&
                            (x.Positions.Contains(position) || position == null) && x.IsGeneral == false).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                jesuscorpers = jesuscorpers.Where(x => x.SearchTag.Contains(search.ToLower())).ToList();

                ViewBag.search = search;
            }

            switch (ppatown)
            {
                case "ezzangbo":
                    ViewBag.ezzangbo = "active";
                    break;
                case "ngbo":
                    ViewBag.ngbo = "active";
                    break;
                case "effium":
                    ViewBag.effium = "active";
                    break;
                default:
                    ViewBag.ezzangbo = "active";
                    break;
            }

            ViewBag.batch = new SelectList(Service.Batch.Get().Where(x => x.Year.Year == year), "BatchName",
                "BatchName", returnBatch);

            return View(jesuscorpers.Where(x => string.Equals(x.PpaTown, ppatown, StringComparison.CurrentCultureIgnoreCase)).ToPagedList(page ?? 1, 12));
        }

        public async Task<ActionResult> CorpMember(int? id)
        {
            var corpmembers = await Service.CorpMember.GetAsync();
            var corpmember = corpmembers.FirstOrDefault(x => x.Id == id);
            if (corpmember == null)
            {
                return HttpNotFound();
            }
            if (corpmember.IsGeneral)
            {
                ViewBag.General = "General";
            }
            return View(corpmember);
        }

        public async Task<ActionResult> OurGenerals(string year, string batch, string position, string search, int? page,
            string ppatown = "ezzangbo")
        {
            var yr = Service.Year.Get();
            yr.Insert(0, new ServiceYear {Id = -2, Year = "All"});
            ViewBag.year = new SelectList(yr, "Year", "Year");

            var portfolio = Service.Portfolio.Get();
            portfolio.Insert(0, new Portfolio {Id = 0, Position = "All"});
            portfolio.Insert(1, new Portfolio {Id = -1, Position = "Excos"});
            ViewBag.position = new SelectList(portfolio, "Position", "Position");

            var returnBatch = batch;

            var pos = position;

            if (year == "All" || year == "") year = null;
            if (batch == "All" || batch == "") batch = null;
            if (position == "All" || position == "Excos" || position == "" || position == null) position = "";

            IEnumerable<CorpMember> generals;

            var corpmembers = await Service.CorpMember.GetAsync();

            if (pos == "Excos")
                generals =
                    corpmembers.Where(
                        x =>
                            (x.ServiceYear.Year == year || year == null) &&
                            (x.Batch.BatchName == batch || batch == null) && x.IsLeader && x.IsGeneral).ToList();
            else
                generals =
                    corpmembers.Where(
                        x =>
                            (x.ServiceYear.Year == year || year == null) &&
                            (x.Batch.BatchName == batch || batch == null) &&
                            (x.Positions.Contains(position) || position == null) && x.IsGeneral).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                generals = generals.Where(x => x.SearchTag.Contains(search.ToLower())).ToList();

                ViewBag.search = search;
            }

            switch (ppatown)
            {
                case "ezzangbo":
                    ViewBag.ezzangbo = "active";
                    break;
                case "ngbo":
                    ViewBag.ngbo = "active";
                    break;
                case "effium":
                    ViewBag.effium = "active";
                    break;
                default:
                    ViewBag.ezzangbo = "active";
                    break;
            }

            ViewBag.batch = new SelectList(Service.Batch.Get().Where(x => x.Year.Year == year), "BatchName",
                "BatchName", returnBatch);

            return View(generals.Where(x => x.PpaTown.ToLower() == ppatown.ToLower()).OrderBy(x => x.FullName).ToPagedList(page ?? 1, 12));
        }

        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public async Task<ActionResult> Gallery(string subzone, string album, string year, string batch, int? page)
        {
            var yr = Service.Year.Get();
            yr.Insert(0, new ServiceYear {Id = -2, Year = "All"});
            ViewBag.year = new SelectList(yr, "Year", "Year");

            var subzones = ListOfThings.ZoneSubZoneList();
            subzones.Insert(0, new SelectListItem {Text = "All", Value = "All"});
            ViewBag.subzone = subzones;

            var returnBatch = batch;
            var returnAlbum = album;

            if (year == "All" || year == "") year = null;
            if (batch == "All" || batch == "") batch = null;
            if (subzone == "All" || subzone == "") subzone = null;
            if (album == "All" || album == "") album = null;

            var albumphotos = await Service.AlbumPhoto.GetAsync();

            var gallery = albumphotos.Where(
                x =>
                    (x.Year.Year == year || year == null) &&
                    (x.Batch.BatchName == batch || batch == null) &&
                    (x.Album.SubZone == subzone || subzone == null) &&
                    (x.Album.Name == album || album == null)).ToList().ToPagedList(page ?? 1, 10);

            ViewBag.batch = new SelectList(Service.Batch.Get().Where(x => x.Year.Year == year), "BatchName",
                "BatchName", returnBatch);

            ViewBag.album = new SelectList(Service.Album.Get().Where(x => x.SubZone == subzone), "Name",
                "Name", returnAlbum);
            return View(gallery);
        }

        public async Task<ActionResult> Projects(string subzone, string project, int? page)
        {
            var subzones = ListOfThings.ZoneSubZoneList();
            subzones.Insert(0, new SelectListItem {Text = "All", Value = "All"});
            ViewBag.subzone = subzones;

            var returnProject = project;

            if (subzone == "All" || subzone == "") subzone = null;
            if (project == "All" || project == "") project = null;

            var projects = await Service.Project.GetAsync();
            var projectmod = projects.Where(
                x =>
                    (x.Name == project || project == null) &&
                    (x.SubZone == subzone || subzone == null)).ToList().ToPagedList(page ?? 1, 10);

            ViewBag.project = new SelectList(Service.Project.Get().Where(x => x.SubZone == subzone), "Name",
                "Name", returnProject);

            return View(projectmod);
        }

    }
}