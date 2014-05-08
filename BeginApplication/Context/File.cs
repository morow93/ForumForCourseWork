using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeginApplication.Context
{
    [Table("File")]
    public partial class File
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }
        public int CommentId { get; set; }
        public byte[] FileContent { get; set; }

        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }
    }
}