using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class AlbumPhoto : EntityBase
    {
        public int AlbumId { get; set; }
        public int YearId { get; set; }
        public int BatchId { get; set; }
        public string ContentType { get; set; }
        public byte[] Photo { get; set; }
        public string Description { get; set; }

        [ForeignKey("AlbumId")]
        public Album Album { get; set; }

        [ForeignKey("YearId")]
        public ServiceYear Year { get; set; }

        [ForeignKey("BatchId")]
        public Batch Batch { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }
    }
}