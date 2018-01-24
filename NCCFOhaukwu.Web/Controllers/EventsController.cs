using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;

namespace NCCFOhaukwu.Web.Controllers {
    public class EventsController : BaseController {
        public EventsController(IService service)
            : base(service) {
        }

        // GET: Events
        public ActionResult Index() {
            return View();
        }

        //[OutputCache(Duration = 60, VaryByParam = "*")]
        [Route("events/{start}/{title}")]
        public async Task<ActionResult> SignleEvent(string title, string start) {
            var events = await Service.Event.GetAsync();
            var singleEvent = events.First(x => Regex.Replace(x.title, "[^a-zA-Z0-9 ]", "").ToLower() == title.Replace("-", " ") &&
                            x.start.ToString("dd/MM/yyyy") == start.Replace("-", "/"));
            return
                View(singleEvent);
        }

        //[OutputCache(Duration = 60, VaryByParam = "*")]
        [Route("events/birthdays")]
        public async Task<ActionResult> Birthdays(string subzone, string month) {
            if (string.IsNullOrEmpty(month)) {
                month = DateTime.Now.ToString("MMMM");
            }
            var subzones = ListOfThings.PpaTownList();
            subzones.Insert(0, new SelectListItem { Text = "All", Value = "All" });
            ViewBag.subzone = subzones;

            if (subzone == "All" || subzone == "") subzone = null;

            var corpmembers = await Service.CorpMember.GetAsync();
            var corpmembersMod = corpmembers.Where(
                x =>
                    (x.Month == month) &&
                    (x.PpaTown == subzone || subzone == null)).ToList();
            ViewBag.selectedMonth = month;
            ViewBag.month = new SelectList(ListOfThings.MonthList(), "Text", "Text", month);
            return View(corpmembersMod);
        }

        private Events EventsList(Event events) {
            return new Events {
                id = events.Id,
                Description = events.Description,
                StartMonth = events.start.ToString("MMMM"),
                StartYear = events.start.Year.ToString(),
                StartDay = events.start.Day.ToString(),
                title = events.title,
                EndTime = events.EndTime.Substring(0, events.EndTime.LastIndexOf(" ", StringComparison.Ordinal)),
                Venue = events.Venue,
                StartTime = events.StartTime.Substring(0, events.StartTime.LastIndexOf(" ", StringComparison.Ordinal)),
                VenueAddress = events.VenueAddress,
                end = "",
                start = events.start.ToString("d"),
                url = events.url,
                StartTimeMeridian =
                    events.StartTime.Substring(events.StartTime.LastIndexOf(" ", StringComparison.Ordinal) + 1),
                EndTimeMeridian =
                    events.EndTime.Substring(events.EndTime.LastIndexOf(" ", StringComparison.Ordinal) + 1)
            };
        }

        [OutputCache(Duration = 60, VaryByParam = "*")]
        [Route("events/getevents")]
        public ActionResult GetEvents(double start, double end) {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);

            //Get the events
            //You may get from the repository also
            var eventList = Service.Event.Get();
            var eventsList = eventList.Select(EventsList);

            var rows = eventsList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        private static DateTime ConvertFromUnixTimestamp(double timestamp) {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
}