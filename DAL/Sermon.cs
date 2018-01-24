using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Sermon : EntityBase
    {
        public string Title { get; set; }
        public string Scriptures { get; set; }
        public string Message { get; set; }

        [Index(IsUnique = true)]
        public DateTime Date { get; set; }
        public string SearchTag { get; set; }
    }
}