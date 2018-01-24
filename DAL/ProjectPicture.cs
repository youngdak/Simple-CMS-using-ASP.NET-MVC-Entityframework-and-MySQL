using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class ProjectPicture : EntityBase
    {
        [Display(Name = "Project Name")]
        public int ProjectId { get; set; }

        public string ContentType { get; set; }
        public byte[] Photo { get; set; }
        public string Description { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}