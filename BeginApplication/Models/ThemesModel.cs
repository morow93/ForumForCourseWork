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
        public string ThemeTitle { get; set; }
        public int ThemeId { get; set; }
    }
}