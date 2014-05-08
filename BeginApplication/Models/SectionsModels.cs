using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeginApplication.Models
{
    public class SectionsModel
    {
        public List<SectionInfo> Sections { get; set; }
    }

    public class SectionInfo
    {
        public int? SectionId { get; set; }
        public string SectionTitle { get; set; }
        public int? ThemeCount { get; set; }
        public int? CommentCount { get; set; }
    }

    public class ChangeSectionModel
    {
        [Required]
        public int SectionId { get; set; }
        [Required]
        public String SectionTitle { get; set; }
        public String Target { get; set; }
    }
}