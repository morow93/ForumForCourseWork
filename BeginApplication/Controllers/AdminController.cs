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

namespace BeginApplication.Controllers
{
    [Authorize(Roles = "admin")]
    [InitializeSimpleMembership]
    public class AdminController : Controller
    {
        public IForumRepository repository;

        public AdminController(IForumRepository _repository)
        {
            repository = _repository;
        }

        public ActionResult Index()
        {
            return View();
        }

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
                case "EmailAsc":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "EmailDesc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                case "UserNameAsc":
                    users = users.OrderBy(u => u.UserName);
                    break;
                case "UserNameDesc":
                    users = users.OrderByDescending(u => u.UserName);
                    break;
                case "RegistrationAsc":
                    users = users.OrderBy(u => u.RegistrationDate);
                    break;
                case "RegistrationDesc":
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

        public string GetChangeRoleUrl(int id, string path)
        {
            var linkText = String.Empty;
            if (Roles.IsUserInRole(repository.UserProfiles.FirstOrDefault(u => u.UserId == id).UserName, "moder"))
            {
                linkText = "<a class='moder-to-user' href='" + Url.Action("ChangeRole", "Admin", new { id = id, role = String.Empty, path = path }, "http") + "'>Модератор -> Пользователь</a>";
            }
            else
            {
                linkText = "<a class='user-to-moder' href='" + Url.Action("ChangeRole", "Admin", new { id = id, role = "moder", path = path }, "http") + "'>Пользователь -> Модератор</a>";
            }
            return linkText;
        }

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

        public ActionResult RemoveUser(UserModel user)
        {
            var name = user.UserName;
            var result = true;
            try 
            {
                var roles = Roles.GetRolesForUser(user.UserName);
                if (roles != null && roles.Length != 0)
                {
                    Roles.RemoveUserFromRoles(user.UserName, roles);
                }
      
                using (var context = new SimpleMembershipContext())
                {
                    var toRemove = context.UserProfiles.FirstOrDefault(u => u.UserId == user.UserId);

                    toRemove.UserName = null;
                    toRemove.Email = null;
                    toRemove.ImageData = null;
                    toRemove.ImageMimeType = null;
                    toRemove.Mobile = null;
                    toRemove.IsDeleted = true;                    

                    context.SaveChanges();
                }
            }
            catch
            {
                result = false;
            }
            if (result)
            {
                TempData["message"] = "Пользователь " + name + " был удален.";
            }
            else
            { 
                TempData["message"] = "При удаленни пользователя " + name + " произошла ошибка.";
            }

            user = null;
            return PartialView("_User", user);
        }
    }
}
