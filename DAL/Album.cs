using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Album : EntityBase
    {
        [Index(IsUnique = true)]
        [StringLength(250)]
        [Display(Name = "Album Name")]
        public string Name { get; set; }

        [Display(Name = "Sub Zone")]
        public string SubZone { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }
        public string SearchTag { get; set; }
    }
}