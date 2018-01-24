using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Project : EntityBase
    {
        [Index(IsUnique = true)]
        [StringLength(250)]
        public string Name { get; set; }

        public string SubZone { get; set; }
        public string Milestone { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string SearchTag { get; set; }
    }
}