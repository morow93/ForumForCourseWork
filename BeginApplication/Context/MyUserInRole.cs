using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeginApplication.Context
{
    [Table("webpages_UsersInRoles")]
    public class MyUserInRole
    {
        [Key]
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}