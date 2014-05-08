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
            return View(new ThemesModel { Themes = repository.GetRecentThemes(5) });    
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }
    }
}
