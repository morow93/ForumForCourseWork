using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeginApplication.Models
{
    public class PrivatePropertiesModel
    {
        [Display(Name="Показывать почту")]
        public bool ShowEmail { get; set; }
        [Display(Name="Телефон")]
        public string Mobile { get; set; }
        [Display(Name="Показывать телефон")]
        public bool ShowMobile { get; set; }
    }
}