using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeginApplication.Models
{
    public class CommentsWithThemeModel
    {
        public CurrentTheme Theme { get; set; }
        public PagedList<CommentInfo> PagedComments { get; set; }
        public int TotalItems { get; set; }
    }

    public class CommentInfo
    {
        public int UserId { get; set; }
        public String UserName { get; set; }
        public List<string> Roles { get; set; }
        public DateTime CreationDate { get; set; }
        public int CommentId { get; set; }
        public String CommentText { get; set; }
        public int? CommentVote { get; set; }
        public bool isAvatarExists { get; set; }
        public bool isAdmitted { get; set; }

        public CommentInfo(CommentInfo model)
        {
            this.UserId = model.UserId;
            this.UserName = model.UserName;
            this.CreationDate = model.CreationDate;
            this.CommentId = model.CommentId;
            this.CommentText = model.CommentText;
            this.CommentVote = model.CommentVote;
            this.isAvatarExists = model.isAvatarExists;
        }
        public CommentInfo() { }
    }

    public class CurrentTheme
    {
        public int UserId { get; set; }
        public String UserName { get; set; }
        public List<String> Roles { get; set; }
        public DateTime CreationDate { get; set; }
        public String ThemeTitle { get; set; }
        public String ThemeText { get; set; }
        public Int32 ThemeId { get; set; }
        public Boolean isAvatarExists { get; set; }
    }
}