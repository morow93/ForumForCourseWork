namespace BeginApplication.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<BeginApplication.Context.SimpleMembershipContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BeginApplication.Context.SimpleMembershipContext context)
        {
            WebSecurity.InitializeDatabaseConnection(
                "SimpleMembershipConnection",
                "UserProfile",
                "UserId",
                "UserName", autoCreateTables: true);

            if (!Roles.RoleExists("admin"))
                Roles.CreateRole("admin");

            if (!WebSecurity.UserExists("derchepur"))
                WebSecurity.CreateUserAndAccount(
                    "derchepur",
                    "1crbvbyjr1",
                    new { Email = "derchepur@gmail.com", Mobile = "+79188539107" });

            if (!Roles.GetRolesForUser("derchepur").Contains("admin"))
                Roles.AddUsersToRoles(new[] { "derchepur" }, new[] { "admin" });
        }
    }
}
