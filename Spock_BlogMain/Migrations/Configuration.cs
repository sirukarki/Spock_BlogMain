namespace Spock_BlogMain.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Spock_BlogMain.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Spock_BlogMain.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Spock_BlogMain.Models.ApplicationDbContext context)
        {



            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //Create Admin and Moderator
            #region roleManager

            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }
            #endregion roleManager


            // i need to create a few users that will eventually occupy the roles of either Admin or moderator

            var userManager = new UserManager<ApplicationUser>(

                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "sirukarki@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sirukarki@Mailinator.com",
                    Email = "sirukarki@Mailinator.com",
                    FirstName = "Srijana",
                    LastName = "Karki",
                    DisplayName = "Srijana"

                }, "Abc&123");


            }
            if (!context.Users.Any(u => u.Email == "sirukarki@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sirukarki@Mailinator.com",
                    Email = "sirukarki@Mailinator.com.com",
                    FirstName = "Srijana",
                    LastName = "Karki",
                    DisplayName = "Srijana"
                }, "Abc&123");


            }


                var userId = userManager.FindByEmail("sirukarki@Mailinator.com").Id;
                userManager.AddToRole(userId, "Admin");
                userId = userManager.FindByEmail("sirukarki@Mailinator.com.com").Id;
                userManager.AddToRole(userId, "Moderator");
        }
}


    }