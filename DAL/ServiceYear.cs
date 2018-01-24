using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class ServiceYear : EntityBase
    {
        [Index(IsUnique = true)]
        [StringLength(9)]
        [Required]
        [Display(Name = "Service Year")]
        public string Year { get; set; }
        public string SearchTag { get; set; }
    }
}