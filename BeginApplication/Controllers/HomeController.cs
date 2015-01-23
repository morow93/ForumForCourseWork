using BeginApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using BeginApplication.Context;
using BeginApplication.Repository;
using BeginApplication.Filters;
using WebMatrix.WebData;
using System.Web.Security;

namespace BeginApplication.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        public IForumRepository repository;

        public HomeController(IForumRepository _repository)
        {
            repository = _repository;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            int? curUserId = null;
            if (Request.IsAuthenticated && WebSecurity.UserExists(WebSecurity.CurrentUserName)) curUserId = WebSecurity.CurrentUserId;
            ;
            var model = repository.GetRecentThemes(5, curUserId);

            var roles = (SimpleRoleProvider)Roles.Provider;
            foreach (var item in model)
            {
                item.Roles = item.UserName == null ? new List<string>() : roles.GetRolesForUser(item.UserName).ToList();
            }

            return View(new ThemesModel { Themes = model });    
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }
    }
}
