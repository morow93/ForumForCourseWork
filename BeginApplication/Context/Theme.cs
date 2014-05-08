using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeginApplication.Context
{
    [Table("Theme")]
    public class Theme
    {
        public Theme()
        {
            this.Comment = new HashSet<Comment>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ThemeId { get; set; }
        public int SectionId { get; set; }
        public int UserId { get; set; }
        public string ThemeTitle { get; set; }
        public string ThemeText { get; set; }
        public System.DateTime CreationDate { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }

        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
    }
}