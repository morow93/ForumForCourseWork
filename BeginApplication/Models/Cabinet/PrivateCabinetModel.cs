using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeginApplication.Models
{
    public class PrivateCabinetModel
    {
        public int UserRating { get; set; }
        public byte[] Image { get; set; }
        public string Mime { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}