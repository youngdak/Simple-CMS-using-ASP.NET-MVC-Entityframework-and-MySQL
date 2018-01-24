using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Batch : EntityBase
    {
        [Display(Name = "Service Year")]
        [Required]
        public int YearId { get; set; }

        [Display(Name = "Batch")]
        [Required]
        public string BatchName { get; set; }

        [ForeignKey("YearId")]
        public ServiceYear Year { get; set; }
        public string SearchTag { get; set; }
    }
}