using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeginApplication.Context
{
    [Table("Message")]
    public partial class Message
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int IdMessage { get; set; }
        public int UserIdFrom { get; set; }
        public int UserIdTo { get; set; }
        public string MessageTitle { get; set; }
        public string MessageText { get; set; }
        public System.DateTime CreationDate { get; set; }

        [ForeignKey("UserIdFrom")]
        public virtual UserProfile UserFrom { get; set; }
        [ForeignKey("UserIdTo")]
        public virtual UserProfile UserTo { get; set; }
    }
}