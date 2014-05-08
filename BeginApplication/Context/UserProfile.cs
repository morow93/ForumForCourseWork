using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeginApplication.Context
{
    [Table("UserProfile")]
    public class UserProfile
    {
        public UserProfile()
        {
            this.Like = new HashSet<Like>();
            this.MessageFrom = new HashSet<Message>();
            this.MessageTo = new HashSet<Message>();
            this.Theme = new HashSet<Theme>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        public byte[] ImageData { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; } 

        public DateTime RegistrationDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Like> Like { get; set; }
        public virtual ICollection<Message> MessageFrom { get; set; }
        public virtual ICollection<Message> MessageTo { get; set; }
        public virtual ICollection<Theme> Theme { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProperty UserProperty { get; set; }
    }
}