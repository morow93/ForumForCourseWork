using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeginApplication.Context
{
    [Table("Comment")]
    public class Comment
    {
        public Comment()
        {
            this.Like = new HashSet<Like>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public int ThemeId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool IsAdmitted { get; set; }

        [ForeignKey("ThemeId")]
        public virtual Theme Theme { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
        public virtual ICollection<Like> Like { get; set; }
    }
}