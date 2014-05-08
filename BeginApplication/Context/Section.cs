using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeginApplication.Context
{
    [Table("Section")]
    public class Section
    {
        public Section()
        {
            this.Theme = new HashSet<Theme>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SectionId { get; set; }
        public string SectionTitle { get; set; }

        public virtual ICollection<Theme> Theme { get; set; }
    }
}