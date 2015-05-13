namespace HouseHoldBuget.Migrations
{
    using HouseHoldBuget.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HouseHoldBuget.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(HouseHoldBuget.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            if (!context.Users.Any(r => r.Email == "markegaines@gmail.com"))
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                userManager.Create(new ApplicationUser
                {
                    UserName = "markegaines@gmail.com",
                    Email = "markegaines@gmail.com",
                    EmailConfirmed = true,
                }, "Plugh4!");
            }
        }
    }
}
