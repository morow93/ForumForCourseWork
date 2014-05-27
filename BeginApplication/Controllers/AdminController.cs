using BeginApplication.Context;
using BeginApplication.Filters;
using BeginApplication.Models;
using BeginApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using PagedList;
using BeginApplication.Helpers;
using System.Net;
using BeginApplication.Models.Paged;

namespace BeginApplication.Controllers
{
    [Authorize(Roles = "admin,moder")]
    [InitializeSimpleMembership]
    public class AdminController : Controller
    {
        public IForumRepository repository;

        public AdminController(IForumRepository _repository)
        {
            repository = _repository;
        }

        [Authorize(Roles = "admin,moder")]
        public ActionResult Index()
        {
            return View();
        }

        #region Получить данные для изменения

        [Authorize(Roles = "admin")]
        public ActionResult GetUsers(string sortOrder, string currentFilter, string searchString, int page = 1)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.EmailSort = (sortOrder == "EmailAsc") ? "EmailDesc" : "EmailAsc";
            ViewBag.UserNameSort = (sortOrder == "UserNameAsc") ? "UserNameDesc" : "UserNameAsc";
            ViewBag.RegistrationSort = (sortOrder == "RegistrationAsc") ? "RegistrationDesc" : "RegistrationAsc";
            
            if (searchString == null)  
                searchString = currentFilter;             
            ViewBag.CurrentFilter = searchString;

            var admins = Roles.GetUsersInRole("admin");
            var users = repository.UserProfiles.Where(u => !u.IsDeleted && !admins.Contains(u.UserName));
   
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s =>
                s.UserName.ToUpper().Contains(searchString.ToUpper())
                ||
                s.Email.ToUpper().Contains(searchString.ToUpper())
                );
            } 
            
            switch (sortOrder)
            {
                case "EmailDesc":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "EmailAsc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                case "UserNameDesc":
                    users = users.OrderBy(u => u.UserName);
                    break;
                case "UserNameAsc":
                    users = users.OrderByDescending(u => u.UserName);
                    break;
                case "RegistrationDesc":
                    users = users.OrderBy(u => u.RegistrationDate);
                    break;
                case "RegistrationAsc":
                    users = users.OrderByDescending(u => u.RegistrationDate);
                    break;
                default:
                    users = users.OrderBy(s => s.UserName);
                    break;
            }          

            var model = new ManagementUsersModel
            {
                TotalItems = users.Count(),
                PagedUsers = (PagedList<UserModel>)users.Select(x => new UserModel
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Email = x.Email,
                    RegistrationDate = x.RegistrationDate
                }).ToPagedList(page, 10)
            };
            
            int countPages = (int)Math.Ceiling((double)model.TotalItems/(double)10);

            if (countPages != 0 && page > countPages)
            {
                while (page > countPages) page--;
                return RedirectToAction("GetUsers", "Admin", new { 
                    sortOrder = sortOrder, 
                    currentFilter = currentFilter, 
                    searchString = searchString, 
                    page = page 
                });
            }
    
            return View(model);  
        }

        [Authorize(Roles = "admin")]
        public ActionResult GetSections()
        {
            return View(repository.Sections.Select(x => new ChangeSectionModel { SectionId = x.SectionId, SectionTitle = x.SectionTitle }).ToList());
        }        

        [Authorize(Roles = "admin,moder")]
        public ActionResult GetNotAdmittedComments(int page = 1)
        {
            var notAdmittedComments = repository.GetNotAdmittedComments();
            var model = new CommentsAdmittedModel { PagedComments = (PagedList<CommentAdmittedInfo>)notAdmittedComments.ToPagedList(page, 10), TotalItems = notAdmittedComments.Count };

            int countPages = (int)Math.Ceiling((double)model.TotalItems / (double)10);
            if (countPages != 0 && page > countPages)
            {
                while (page > countPages) page--;
                return RedirectToAction("GetNotAdmittedComments", "Admin", new { page = page });
            }
            
            return View(model);
        }

        #endregion

        #region Модерация сообщений

        [Authorize(Roles = "admin,moder")]
        public ActionResult RemoveComment(int id, string path)
        {
            var result = repository.RemoveComment(id);
            if (result)
                TempData["success"] = "Комментарий был удален.";
            else
                TempData["failure"] = "Комментарий не был удален.";

            if (path.RemoteFileExists())
                return Redirect(path);
            else
                return RedirectToAction("GetNotAdmittedComments", "Admin", new { page = Math.Ceiling((double)repository.Comments.Where(c => !c.IsAdmitted).Count() / 10) });
        }

        [Authorize(Roles = "admin,moder")]
        public ActionResult AdmittComment(int id, string path)
        {
            var result = repository.AdmittComment(id);
            if (result)
                TempData["success"] = "Комментарий был допущен.";
            else
                TempData["failure"] = "Комментарий не был допущен.";

            if (path.RemoteFileExists())
                return Redirect(path);
            else
                return RedirectToAction("GetNotAdmittedComments", "Admin", new { page = Math.Ceiling((double)repository.Comments.Where(c => !c.IsAdmitted).Count() / 10) });
        }

        #endregion

        #region Переименование раздела

        [Authorize(Roles = "admin")]
        public ActionResult _RenameSection(ChangeSectionModel section)
        {
            return PartialView(section);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult _SubmitSectionChange(ChangeSectionModel section)
        {
            using (var context = new SimpleMembershipContext())
            { 
                var db_section = context.Sections.FirstOrDefault(x => x.SectionId == section.SectionId);

                if (ModelState.IsValid)
                {
                    try
                    {
                        db_section.SectionTitle = section.SectionTitle;
                        context.SaveChanges();
                    }
                    catch
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    section.SectionTitle = db_section.SectionTitle;
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }               
            }
            return PartialView("_Section", section);
        }

        [Authorize(Roles = "admin")]
        public ActionResult _HideRenameForm(ChangeSectionModel section)
        {
            return PartialView("_Section", section);
        }

        #endregion

        #region Изменить роль пользователя

        [Authorize(Roles = "admin")]
        public string GetChangeRoleUrl(int id, string path)
        {
            var linkText = String.Empty;
            if (Roles.IsUserInRole(repository.UserProfiles.FirstOrDefault(u => u.UserId == id).UserName, "moder"))
            {
                linkText = "<a class='moder-to-user' title='Разжаловать' href='" 
                    + Url.Action("ChangeRole", "Admin", new { id = id, role = String.Empty, path = path }, "http") 
                    + "'>Модератор -> Пользователь</a>";
            }
            else
            {
                linkText = "<a class='user-to-moder' title='Повысить' href='" 
                    + Url.Action("ChangeRole", "Admin", new { id = id, role = "moder", path = path }, "http") 
                    + "'>Пользователь -> Модератор</a>";
            }
            return linkText;
        }

        [Authorize(Roles = "admin")]
        public ActionResult ChangeRole(int id, string role, string path)
        {
            if (string.IsNullOrEmpty(role))            
                Roles.RemoveUserFromRole(repository.UserProfiles.FirstOrDefault(u => u.UserId == id).UserName, "moder");
            else
                Roles.AddUserToRole(repository.UserProfiles.FirstOrDefault(u => u.UserId == id).UserName, "moder");

            if (path.RemoteFileExists())
                return Redirect(path);
            else
                return RedirectToAction("GetUsers", "Admin");
        }

        #endregion

        /// <summary>
        /// Удаление юзера
        /// </summary>
        [Authorize(Roles = "admin")]
        public ActionResult RemoveUser(UserModel user)
        {
            if (ModelState.IsValid)
            { 
                try
                {
                    Roles.RemoveUserFromRole(user.UserName, "active");
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }                
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return PartialView("_UserManage", user);
        }

        /// <summary>
        /// Восстановление юзера
        /// </summary>
        [Authorize(Roles = "admin")]
        public ActionResult RecoveryUser(UserModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Roles.AddUserToRole(user.UserName, "active");
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }  
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return PartialView("_UserManage", user);
        }

        /// <summary>
        /// Удаление раздела
        /// </summary>
        [Authorize(Roles = "admin")]
        public ActionResult RemoveSection(ChangeSectionModel section)
        {
            if (ModelState.IsValid)
            {
                var result = repository.RemoveSection(section.SectionId);

                if (result)
                {
                    section = null;
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return PartialView("_Section", section);
        }

        /// <summary>
        /// Создание нового раздела
        /// </summary>
        [Authorize(Roles = "admin")]
        public ActionResult CreateSection(ChangeSectionModel model)
        {
            if (ModelState.IsValid)
            {
                var result = repository.AddSection(new Section { SectionTitle = model.SectionTitle });
                
                if (!result)
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return PartialView("_CreateSection", null);
        }
    }
}
