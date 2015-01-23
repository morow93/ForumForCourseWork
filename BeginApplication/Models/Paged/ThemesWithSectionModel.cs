using BeginApplication.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeginApplication.Models
{
    public class ThemesWithSectionModel
    {
        public PagedList<ThemeInfo> PagedThemes { get; set; }
        public Section Section { get; set; }
        public int TotalItems { get; set; }

        public string SearchingString { get; set; }
    }

    public class ThemeInfo
    {
        public int UserId { get; set; }
        public String UserName { get; set; }
        public List<String> Roles { get; set; }
        public DateTime CreationDate { get; set; }
        public String ThemeTitle { get; set; }
        public int ThemeId { get; set; }
        public int CountComments { get; set; }
        public int CountUncheckedComments { get; set; }
    }
}