using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeginApplication.Models
{
    public class PrivateCabinetModel
    {
        public int UserRating { get; set; }
        public bool isAvatarExist { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}