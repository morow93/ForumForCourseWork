using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeginApplication.Context
{
    [Table("Like")]
    public partial class Like
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int LikeId { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public int Vote { get; set; }

        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
    }
}