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
            
            return View(new ThemesModel { Themes = repository.GetRecentThemes(5, curUserId) });    
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }
    }
}
