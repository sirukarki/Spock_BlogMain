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

            if (!context.Users.Any(u => u.Email == "JoeSnow@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "JoeSnow@Mailinator.com",
                    Email = "JoeSnow@Mailinator.com",
                    FirstName = "Joey",
                    LastName = "Snow",
                    DisplayName = "Joe"

                }, "Abc&123");


            }
            if (!context.Users.Any(u => u.Email == "HappySnow@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "HappySnow@Mailinator.com",
                    Email = "HappySnow@Mailinator.com",
                    FirstName = "Happy",
                    LastName = "Snow",
                    DisplayName = "Hap"
                }, "Abc&123");


            }


            var userId = userManager.FindByEmail("JoeSnow@Mailinator.com").Id;
                userManager.AddToRole(userId, "Admin");
                userId = userManager.FindByEmail("HappySnow@Mailinator.com").Id;
                userManager.AddToRole(userId, "Moderator");
        }
    }
}

