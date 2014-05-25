using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeginApplication.Models
{
    public class ThemesModel
    {
        public List<ThemeInfo> Themes { get; set; }
    }

    public class ShortThemeInfo
    {
        public DateTime CreationDate { get; set; }
        public string ThemeTitle { get; set; }
        public int ThemeId { get; set; }
    }

    public class UserThemesModel
    {
        public PagedList<ShortThemeInfo> PagedThemes { get; set; }
        public int TotalItems { get; set; }
        public int UserId { get; set; }
    }
}