using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeginApplication.Models.Paged
{
    public class CommentAdmittedInfo
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreationDate { get; set; }

        public int ThemeId { get; set; }
        public string ThemeTitle { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class CommentsAdmittedModel
    {
        public PagedList<CommentAdmittedInfo> PagedComments { get; set; }
        public int TotalItems { get; set; }
    }
}