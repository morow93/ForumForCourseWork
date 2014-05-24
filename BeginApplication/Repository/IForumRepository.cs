﻿using BeginApplication.Context;
using BeginApplication.Models;
using BeginApplication.Models.Paged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeginApplication.Repository
{
    public interface IForumRepository
    {
        IQueryable<UserProfile> UserProfiles { get; }
        IQueryable<UserProperty> UserProperties { get; }
        IQueryable<Section> Sections { get; }
        IQueryable<Theme> Themes { get; }
        IQueryable<Comment> Comments { get; }
        IQueryable<Like> Likes { get; }
        
        List<SectionInfo> GetForumSections();
        List<ThemeInfo> GetRecentThemes(int count);        
        List<ThemeInfo> GetThemesBySection(int id);
        List<CommentInfo> GetCommentsByTheme(int id, bool isModer);
        List<CommentAdmittedInfo> GetNotAdmittedComments();

        void AddTheme(Theme theme);
        void AddComment(Comment comment);
        bool AddSection(Section section);

        PrivatePropertiesModel GetPrivateProperties(int id);
        void SetPrivateProperties(PrivatePropertiesModel model, int id);
        PrivateCabinetModel GetPrivateCabinet(int userId);

        bool RemoveTheme(int id);
        bool RemoveComment(int id);
        bool RemoveUser(UserModel user);
        bool RemoveSection(int id);

        UserSummaryModel GetUserSummary(int id);
        bool AdmittComment(int id);
    }
}