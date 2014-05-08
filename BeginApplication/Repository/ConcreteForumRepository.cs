﻿using BeginApplication.Context;
using BeginApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace BeginApplication.Repository
{
    public class ConcreteForumRepository: IForumRepository
    {
        #region Таблицы

        private SimpleMembershipContext context = new SimpleMembershipContext();

        public IQueryable<UserProfile> UserProfiles
        {
            get { return context.UserProfiles; }
        }

        public IQueryable<UserProperty> UserProperties
        {
            get { return context.UserProperties; }
        }

        public IQueryable<Section> Sections
        {
            get { return context.Sections; }
        }

        public IQueryable<Theme> Themes
        {
            get { return context.Themes; }
        }

        public IQueryable<Comment> Comments
        {
            get { return context.Comments; }
        }

        public IQueryable<File> Files
        {
            get { return context.Files; }
        }

        public IQueryable<Like> Likes
        {
            get { return context.Likes; }
        }

        public IQueryable<Message> Messages
        {
            get { return context.Messages; }
        }

        #endregion

        #region Выборка для форума

        public List<ThemeInfo> GetRecentThemes(int count)
        {
            return (from themes in context.Themes
                    join users in context.UserProfiles on themes.UserId equals users.UserId
                    into usersGroup
                    from users in usersGroup.DefaultIfEmpty()
                    join comments in context.Comments on themes.ThemeId equals comments.ThemeId
                    into commentsGroup
                    from comments in commentsGroup.DefaultIfEmpty()
                    group new { themes, users, comments }
                    by new { users.UserId, users.UserName, themes.ThemeId, themes.ThemeTitle, themes.CreationDate }
                    into groupResult
                    select new ThemeInfo
                    {
                        UserId = groupResult.Key.UserId,
                        UserName = groupResult.Key.UserName,
                        ThemeId = groupResult.Key.ThemeId,
                        ThemeTitle = groupResult.Key.ThemeTitle,
                        CreationDate = groupResult.Key.CreationDate,
                        CountComments = groupResult.Select(c => c.comments.CommentId).Count(x => x != null)
                    }).OrderByDescending(ti => ti.CreationDate).Take(count).ToList();
        }

        public List<SectionInfo> GetForumSections()
        {
            var query = (from sections in context.Sections
                         join themes in context.Themes on sections.SectionId equals themes.SectionId 
                         into themesGroup from themes in themesGroup.DefaultIfEmpty()
                         join comments in context.Comments on themes.ThemeId equals comments.ThemeId 
                         into commentGroup from comments in commentGroup.DefaultIfEmpty()
                         group new { sections, themes, comments } by new { sections.SectionId, sections.SectionTitle } 
                         into groupResult
                         select new SectionInfo
                         {
                             SectionId = groupResult.Key.SectionId,
                             SectionTitle = groupResult.Key.SectionTitle,
                             ThemeCount = groupResult.Select(s => s.themes.ThemeId).Distinct().Count(x => x != null),
                             CommentCount = groupResult.Select(s => s.comments.CommentId).Count(x => x != null)
                         }).OrderBy(x => x.SectionId).ToList();
            return query;
        }

        public List<ThemeInfo> GetThemesBySection(int id)
        {
            return (from themes in context.Themes
                    join users in context.UserProfiles on themes.UserId equals users.UserId
                    into usersGroup
                    from users in usersGroup.DefaultIfEmpty()
                    join comments in context.Comments on themes.ThemeId equals comments.ThemeId
                    into commentsGroup
                    from comments in commentsGroup.DefaultIfEmpty()
                    where themes.SectionId == id
                    group new { themes, users, comments }
                    by new { users.UserId, users.UserName, themes.ThemeId, themes.ThemeTitle, themes.CreationDate }
                    into groupResult
                    select new ThemeInfo
                    {
                        UserId = groupResult.Key.UserId,
                        UserName = groupResult.Key.UserName,
                        ThemeId = groupResult.Key.ThemeId,
                        ThemeTitle = groupResult.Key.ThemeTitle,
                        CreationDate = groupResult.Key.CreationDate,
                        CountComments = groupResult.Select(c => c.comments.CommentId).Count(x => x != null)                             
                    }).OrderByDescending(ti => ti.CreationDate).ToList();
        }

        public List<CommentInfo> GetCommentsByTheme(int id)
        {
            return context.Comments.Where(c => c.ThemeId == id).Select(x => new CommentInfo
                    {
                        CommentId = x.CommentId,
                        CommentText = x.CommentText,
                        CreationDate = x.CreationDate,
                        UserName = x.User.UserName,
                        UserId = x.User.UserId,
                        CommentVote = x.Like.Where(y => y.CommentId == x.CommentId).Sum(z => z.Vote) == null ? 0 : x.Like.Where(y => y.CommentId == x.CommentId).Sum(z => z.Vote)
                    }).OrderBy(c => c.CreationDate).ToList();
        }

        #endregion

        #region Вставка

        public void AddTheme(Theme theme)
        {
            try
            {
                context.Themes.Add(theme);
                context.SaveChanges();
            }
            catch { }
        }

        public void AddComment(Comment comment)
        {
            try
            {
                context.Comments.Add(comment);
                context.SaveChanges();
            }
            catch { }
        }

        #endregion

        #region Настройки приватности

        public PrivatePropertiesModel GetPrivateProperties(int id)
        {
            var model = new PrivatePropertiesModel();

            var isExist = context
                .UserProperties
                .FirstOrDefault(x => x.UserId == id);

            if (isExist == null)
            {
                model.ShowEmail = model.ShowMobile = false;
            }
            else
            {
                model.ShowEmail = isExist.ShowEmail;
                model.ShowMobile = isExist.ShowMobile;
            }

            var mobile = context.UserProfiles.FirstOrDefault(x => x.UserId == id).Mobile;
            model.Mobile = mobile == null ? String.Empty : mobile;

            return model;
        }

        public void SetPrivateProperties(PrivatePropertiesModel model, int id)
        {
            var existProperty = context.UserProperties.FirstOrDefault(x => x.UserId == id);

            if (existProperty == null)
            {
                context.UserProperties.Add(new UserProperty
                {
                    UserId = id,
                    AllowSendMessage = false,
                    ShowEmail = (bool)model.ShowEmail,
                    ShowMobile = (bool)model.ShowMobile
                });
            }
            else
            {
                existProperty.ShowEmail = model.ShowEmail;
                existProperty.ShowMobile = model.ShowMobile;
            }

            var existProfile = context.UserProfiles.FirstOrDefault(x => x.UserId == id);
            existProfile.Mobile = model.Mobile;

            context.SaveChanges();
        }

        

        public PrivateCabinetModel GetPrivateCabinet(int userId)
        {
            var user = context.UserProfiles.FirstOrDefault(u => u.UserId == userId);
            var result = new PrivateCabinetModel
            {
                Image = user.ImageData,
                Mime = user.ImageMimeType,
                RegistrationDate = user.RegistrationDate                
            };         
            var query = context.Likes.Where(lk=>lk.Comment.UserId == userId);
            if (query == null || query.Count() == 0)            
                result.UserRating = 0;            
            else            
                result.UserRating = query.Sum(x => x.Vote);
            
            return result;
        }

        #endregion

        #region Удаление

        public bool RemoveTheme(int id)
        {
            var theme = context.Themes.FirstOrDefault(u => u.ThemeId == id);
            var result = true;

            if (theme != null)
            {                
                try 
                {
                    foreach (var comment in theme.Comment.ToList())
                    {
                        foreach (var like in comment.Like.ToList())
                        {
                            context.Likes.Remove(like);
                        }
                        context.Comments.Remove(comment);
                    }
                    context.Themes.Remove(theme);

                    context.SaveChanges();
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        public bool RemoveComment(int id)
        {
            var comment = context.Comments.FirstOrDefault(c => c.CommentId == id);
            var result = true;

            if (comment != null)
            {
                try
                {
                    foreach (var like in comment.Like.ToList())
                    {
                        context.Likes.Remove(like);
                    }
                    context.Comments.Remove(comment);

                    context.SaveChanges();
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        #endregion
    }
}