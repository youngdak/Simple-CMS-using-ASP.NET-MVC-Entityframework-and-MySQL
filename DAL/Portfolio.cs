using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Portfolio : EntityBase
    {
        [Index(IsUnique = true)]
        [StringLength(250)]
        public string Position { get; set; }
        public string SearchTag { get; set; }
    }
}