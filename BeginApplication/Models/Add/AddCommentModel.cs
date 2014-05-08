using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeginApplication.Models
{
    public class AddCommentModel
    {
        [Required]
        public int ThemeId { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Содержание комментария")]
        [StringLength(maximumLength: 3000)]
        public string CommentText { get; set; }
    }
}