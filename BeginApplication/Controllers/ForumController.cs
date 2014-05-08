using BeginApplication.Context;
using BeginApplication.Filters;
using BeginApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using BeginApplication.Models;
using WebMatrix.WebData;
using PagedList;
using System.Threading.Tasks;
using BeginApplication.Helpers;
using System.Web.Security;

namespace BeginApplication.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class ForumController : Controller
    {
        public IForumRepository repository;

        public ForumController(IForumRepository _repository)
        {
            repository = _repository;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(new SectionsModel { Sections = repository.GetForumSections() });
        }
        
        [AllowAnonymous]
        public ActionResult Section(int id, int page = 1)
        {
            var themesBySection = repository.GetThemesBySection(id);
            var model = new ThemesWithSectionModel
            {
                TotalItems = themesBySection.Count,
                PagedThemes = (PagedList<ThemeInfo>)themesBySection.ToPagedList(page, 10),
                Section = repository.Sections.FirstOrDefault(s => s.SectionId == id)
            };

            int countPages = (int)Math.Ceiling((double)model.TotalItems/(double)10);
            if (countPages != 0 && page > countPages)
            {
                while (page > countPages) page--;
                return RedirectToAction("Section", "Forum", new { id = id, page = page });
            }
            return View(model);
        }
        
        [AllowAnonymous]
        public ActionResult Theme(int id, int page = 1)
        {
            var commentsByTheme = repository.GetCommentsByTheme(id);
            var model = new CommentsWithThemeModel
            {
                TotalItems = commentsByTheme.Count,
                PagedComments = (PagedList<CommentInfo>)commentsByTheme.ToPagedList(page, 10),
                Theme = repository.Themes.Where(t => t.ThemeId == id).Select(x => new CurrentTheme
                {
                    UserId = x.User.UserId,
                    UserName = x.User.UserName,
                    ThemeId = x.ThemeId,
                    ThemeTitle = x.ThemeTitle,
                    ThemeText = x.ThemeText,
                    CreationDate = x.CreationDate
                }).FirstOrDefault()
            };

            int countPages = (int)Math.Ceiling((double)model.TotalItems/(double)10);
            if (countPages != 0 && page > countPages)
            {
                while (page > countPages) page--;
                return RedirectToAction("Theme", "Forum", new { id = id, page = page });
            }
            return View(model);
        }

        [HttpPost]
        public PartialViewResult _GetCommentData(CommentInfo model)
        {
            if (model.UserId != WebSecurity.CurrentUserId)
            {
                using (var context = new SimpleMembershipContext())
                {
                    var record = context.Likes.FirstOrDefault(l => l.UserId == WebSecurity.CurrentUserId && l.CommentId == model.CommentId);

                    if (record == null)
                    {
                        context.Likes.Add(new Like { CommentId = model.CommentId, UserId = WebSecurity.CurrentUserId, Vote = 1 });
                        model.CommentVote++;
                    }
                    else
                    {
                        if (record.Vote == 0)
                        {
                            record.Vote++;
                            model.CommentVote++;
                        }
                        else
                        {
                            model.CommentVote--;
                            record.Vote--;
                        }
                    }
                    context.SaveChanges();                    
                }
            }
            return PartialView("_GetCommentData", model);
        }

        #region Добавление темы

        public ActionResult AddTheme(int id)
        {
            if (!Roles.IsUserInRole("admin"))
            {
                var isExist = repository.Themes.Where(t => t.UserId == WebSecurity.CurrentUserId).OrderByDescending(t => t.CreationDate).FirstOrDefault();
                if (isExist != null)
                {
                    if ((DateTime.Now - isExist.CreationDate).TotalMinutes < 60)
                    {
                        TempData["failure"] = "Новую тему можно создать один раз в час.";
                        return RedirectToAction("Section", new { id = id, page = 1 });
                    }
                }
            }
            var model = new AddThemeModel { 
                UserId = (int)WebSecurity.CurrentUserId,
                SectionId = id
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddTheme(AddThemeModel theme)
        {
            if (ModelState.IsValid)
            {
                var themeToAdd = new Theme
                {
                    SectionId = theme.SectionId,
                    UserId = theme.UserId,
                    ThemeTitle = theme.ThemeTitle,
                    ThemeText = theme.ThemeText,
                    CreationDate = DateTime.Now
                };
                repository.AddTheme(themeToAdd);
                TempData["success"] = "Новая тема успешно создана.";
            }
            else
            {
                return View(theme);
            }
            return RedirectToAction("Section", "Forum", new { id = theme.SectionId });
        }

        #endregion

        #region Удаление темы

        [Authorize(Roles="admin,moder")]
        public ActionResult RemoveTheme(int id, int section, string path)
        {
            var result = repository.RemoveTheme(id);
            if (result)
                TempData["success"] = "Тема была удалена.";
            else
                TempData["failure"] = "Тема не была удалена.";

            if (path.RemoteFileExists())
                return Redirect(path);
            else
                return RedirectToAction("Section", "Forum", new { id = section, page = Math.Ceiling((double)repository.Themes.Where(t=>t.SectionId == section).Count()/10)});
        }

        #endregion

        #region Добавление комментария

        public PartialViewResult _AddComment(int th)
        {
            var model = new AddCommentModel
            {
                UserId = (int)WebSecurity.CurrentUserId,
                ThemeId = th
            };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult _AddComment(AddCommentModel comment)
        {
            if (ModelState.IsValid)
            {
                var ToAdd = new Comment
                {
                    ThemeId = comment.ThemeId,
                    UserId = comment.UserId,
                    CommentText = comment.CommentText,
                    CreationDate = DateTime.Now
                };
                repository.AddComment(ToAdd);
                TempData["success"] = "Сообщение создано.";
            }
            else
            {
                TempData["failure"] ="Сообщение не было создано.";
            }
            return RedirectToAction("Theme", "Forum", new { id = comment.ThemeId, page = Math.Ceiling((double)repository.Comments.Where(c=>c.ThemeId == comment.ThemeId).Count()/10) });
        }

        #endregion

        #region Удаление комментария

        [Authorize(Roles="admin,moder")]
        public ActionResult RemoveComment(int id, int theme, string path)
        {
            var result = repository.RemoveComment(id);
            if (result)
                TempData["success"] = "Комментарий был удален.";
            else
                TempData["failure"] = "Комментарий не был удален.";

            if (path.RemoteFileExists())
                return Redirect(path);
            else
                return RedirectToAction("Theme", "Forum", new { id = theme, page = Math.Ceiling((double)repository.Comments.Where(t => t.ThemeId == theme).Count() / 10) });
        }

        #endregion
    }
}
