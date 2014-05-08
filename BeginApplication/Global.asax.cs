using BeginApplication.Repository;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.Mvc4;
using WebMatrix.WebData;

namespace BeginApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebSecurityInitializer.Instance.EnsureInitialize();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Bootstrapper.Initialise();
        }
    }

    /// <summary>
    /// Для потокобезопасной инициализаци системы членства
    /// </summary>
    public class WebSecurityInitializer
    {
        private WebSecurityInitializer() { }
        public static readonly WebSecurityInitializer Instance = new WebSecurityInitializer();
        private bool isNotInit = true;
        private readonly object SyncRoot = new object();
        public void EnsureInitialize()
        {
            if (isNotInit)
            {
                lock (this.SyncRoot)
                {
                    if (isNotInit)
                    {
                        isNotInit = false;
                        WebSecurity.InitializeDatabaseConnection("SimpleMembershipConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    }
                }
            }
        }
    }
}