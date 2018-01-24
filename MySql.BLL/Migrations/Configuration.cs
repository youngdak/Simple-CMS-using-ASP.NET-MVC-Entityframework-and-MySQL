using System;
using System.Data.Entity.Migrations;
using System.Web.Configuration;
using DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MySql.BLL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var email = WebConfigurationManager.AppSettings["email"];
            var password = WebConfigurationManager.AppSettings["access"];
            var roleName = WebConfigurationManager.AppSettings["role"];

            var o2r = new OperationToRoles
            {
                Operations = Operations.All,
                ResourceId = Resources.ResourceOperation,
                RoleName = roleName,
                ResourceName = "ResourceOperation"
            };

            context.OperationToRoles.AddOrUpdate(o2r);

            var hasher = new PasswordHasher();

            // Add role
            context.Roles.AddOrUpdate(new IdentityRole(roleName));

            // Add user
            context.Users.AddOrUpdate(new ApplicationUser
            {
                UserName = email,
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString("n"),
                PasswordHash = hasher.HashPassword(password)
            });
        }
    }
}