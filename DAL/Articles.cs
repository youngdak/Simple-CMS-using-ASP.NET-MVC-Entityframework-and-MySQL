using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class Articles : EntityBase
    {
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Article { get; set; }
        public bool Display { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [ForeignKey("AuthorId")]
        public CorpMember Author { get; set; }
        public string Username { get; set; }
        public string SearchTag { get; set; }
    }
}