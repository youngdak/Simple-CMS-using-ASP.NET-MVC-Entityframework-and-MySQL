using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DAL
{
    public class CorpMember : EntityBase
    {
        [Display(Name = "Service Year")]
        public int YearId { get; set; }

        [Display(Name = "Batch")]
        public int BatchId { get; set; }

        [Display(Name = "Position(s)")]
        public string Positions { get; set; }

        public string PositionIds { get; set; }

        public string FullName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Other Name")]
        public string OtherName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Index(IsUnique = true)]
        [StringLength(100)]
        public string Username { get; set; }

        public string Gender { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }

        [Display(Name = "State of Origin")]
        public string StateOfOrigin { get; set; }

        public string Lga { get; set; }

        [Required]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        [Display(Name = "Personal Information")]
        [AllowHtml]
        public string PersonalInfo { get; set; }

        [Display(Name = "Academic Information")]
        [AllowHtml]
        public string AcademicInfo { get; set; }

        [Display(Name = "NCCF Information")]
        [AllowHtml]
        public string NccfInfo { get; set; }

        [Display(Name = "PPA Town")]
        public string PpaTown { get; set; }

        public bool IsGeneral { get; set; }
        public bool IsLeader { get; set; }

        [ForeignKey("YearId")]
        public ServiceYear ServiceYear { get; set; }

        [ForeignKey("BatchId")]
        public Batch Batch { get; set; }

        public byte[] MiniImage { get; set; }
        public string MiniContentType { get; set; }
        public byte[] MajorImage { get; set; }
        public string MajorContentType { get; set; }
        public string SearchTag { get; set; }
    }
}