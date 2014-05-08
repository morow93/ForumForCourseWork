using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeginApplication.Models
{
    public class AddThemeModel
    {
        [Required]
        public int SectionId { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Название темы")]
        [StringLength(maximumLength: 100)]
        public string ThemeTitle { get; set; }

        [Required]
        [Display(Name = "Содержание темы")]
        [StringLength(maximumLength: 5000)]
        public string ThemeText { get; set; }
    }
}