using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class EventDbController : BaseController
    {
        public EventDbController(IService service) : base(service)
        {
        }

        public ActionResult GetEvents(double start, double end)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);

            var eventList = Service.Event.Get();

            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        //public async Task<ActionResult> Events()
        //{
        //    return View(new Events());
        //}

        [Access(Resources.Event, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var events = await Service.Event.GetAsync();

            if (string.IsNullOrEmpty(search)) return View(events.ToPagedList(page ?? 1, 10));
            events =
                events.Where(
                    x => x.SearchTag.Contains(search.ToLower())).ToList();

            ViewBag.search = search;

            return View(events.ToPagedList(page ?? 1, 10));
        }
        
        [Access(Resources.Event, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var @event = await Service.Event.GetAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }
        
        [Access(Resources.Event, Operations.Create)]
        public ActionResult Create()
        {
            return View(new Events());
        }
        
        [Access(Resources.Event, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Events events)
        {
            if (ModelState.IsValid)
            {
                var newEvent = new Event
                {
                    title = events.title,
                    start = DateTime.Parse(events.start),
                    StartTime = events.StartTime + " " + events.StartTimeMeridian,
                    EndTime = events.EndTime + " " + events.EndTimeMeridian,
                    Description = events.Description,
                    Venue = events.Venue,
                    VenueAddress = events.VenueAddress,
                    url = "/events/" + events.start.Replace("/", "-") + "/" + Regex.Replace(events.title, "[^a-zA-Z0-9 ]", "").Replace(" ", "-").ToLower(),
                    StartMonth = DateTime.Parse(events.start).ToString("MMMM"),
                    StartYear = DateTime.Parse(events.start).Year.ToString(),
                    StartDay = DateTime.Parse(events.start).Day.ToString(),
                    SearchTag = events.title.ToLower() + events.Description.ToLower() + events.Venue.ToLower() + events.VenueAddress.ToLower()
                };
                await Service.Event.AddAsync(newEvent);
                return RedirectToAction("Index");
                //return Json("true");
            }

            return View(events);
        }
        
        [Access(Resources.Event, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var events = await Service.Event.GetAsync(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            var eventModel = new Events
            {
                id = events.Id,
                Description = events.Description,
                title = events.title,
                EndTime = events.EndTime.Substring(0, events.EndTime.LastIndexOf(" ", StringComparison.Ordinal)),
                Venue = events.Venue,
                StartTime = events.StartTime.Substring(0, events.StartTime.LastIndexOf(" ", StringComparison.Ordinal)),
                VenueAddress = events.VenueAddress,
                start = events.start.ToString("d"),
                url = events.url,
                StartTimeMeridian =
                    events.StartTime.Substring(events.StartTime.LastIndexOf(" ", StringComparison.Ordinal) + 1),
                EndTimeMeridian =
                    events.EndTime.Substring(events.EndTime.LastIndexOf(" ", StringComparison.Ordinal) + 1)
            };
            return View(eventModel);
        }
        
        [Access(Resources.Event, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Events events)
        {
            if (ModelState.IsValid)
            {
                var newEvent = new Event
                {
                    Id = events.id,
                    title = events.title,
                    start = DateTime.Parse(events.start),
                    StartTime = events.StartTime + " " + events.StartTimeMeridian,
                    EndTime = events.EndTime + " " + events.EndTimeMeridian,
                    Description = events.Description,
                    Venue = events.Venue,
                    VenueAddress = events.VenueAddress,
                    url = "/events/" + events.start.Replace("/", "-") + "/" + Regex.Replace(events.title, "[^a-zA-Z0-9 ]", "").Replace(" ", "-").ToLower(),
                    StartMonth = DateTime.Parse(events.start).ToString("MMMM"),
                    StartYear = DateTime.Parse(events.start).Year.ToString(),
                    StartDay = DateTime.Parse(events.start).Day.ToString(),
                    SearchTag = events.title.ToLower() + events.Description.ToLower() + events.Venue.ToLower() + events.VenueAddress.ToLower()
                };
                await Service.Event.UpdateAsync(newEvent);
                return RedirectToAction("Index");
            }
            return View(events);
        }
        
        [Access(Resources.Event, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var @event = await Service.Event.GetAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }
        
        [Access(Resources.Event, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var @event = await Service.Event.GetAsync(id);
            await Service.Event.DeleteAsync(@event);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Event.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}