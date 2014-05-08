using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using BeginApplication.Models;
using BeginApplication.Context;

namespace BeginApplication.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Однократная инициализации при каждом запуске приложения
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<SimpleMembershipContext>(null);

                try
                {
                    using (var context = new SimpleMembershipContext())
                    {
                        if (!context.Database.Exists())
                        {
                            //Без миграции
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }
                    //инициализация WebSecurity вынесена в Global.asax
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Не удалось инициализировать базу данных ASP.NET Simple Membership. Чтобы получить дополнительные сведения, перейдите по адресу: http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
