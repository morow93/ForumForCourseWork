using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeginApplication.Models
{
    public class UserSummaryModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int? Rating { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool isAvatarExists { get; set; }
    }

    public class ShortUserSummary
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}