using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace BeginApplication.Models
{
    public class ManagementUsersModel
    {
        public PagedList<UserModel> PagedUsers { get; set; }
        public int TotalItems { get; set; }
    }

    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string Target { get; set; }
    }
}