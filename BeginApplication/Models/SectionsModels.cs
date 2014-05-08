using System;
using System.Collections.Generic;
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
}