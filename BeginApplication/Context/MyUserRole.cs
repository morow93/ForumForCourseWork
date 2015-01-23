using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeginApplication.Context
{
    [Table("webpages_Roles")]
    public class MyUserRole
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}