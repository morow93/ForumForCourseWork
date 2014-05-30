using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeginApplication.Context
{
    [Table("UserProperty")]
    public class UserProperty
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public bool ShowEmail { get; set; }
        public bool ShowMobile { get; set; }

        [Required]
        public virtual UserProfile User { get; set; }
    }
}