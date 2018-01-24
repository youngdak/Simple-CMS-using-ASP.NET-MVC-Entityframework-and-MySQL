using System;

namespace DAL
{
    public class Event : EntityBase
    {
        public string title { get; set; }
        public string StartDay { get; set; }
        public string StartMonth { get; set; }
        public string StartYear { get; set; }
        public DateTime start { get; set; }
        public string end { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string Venue { get; set; }
        public string VenueAddress { get; set; }
        public string url { get; set; }
        public string SearchTag { get; set; }
    }
}