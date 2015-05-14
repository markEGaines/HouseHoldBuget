namespace HouseHoldBuget.Migrations
{
    using HouseHoldBuget.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Configuration;
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
                    Households = new Household { Name = "markegaines@gmail.com" },
                    UserName = "markegaines@gmail.com",
                    Email = "markegaines@gmail.com",
                    EmailConfirmed = true,
                }, "Plugh4!");
            }

            if (!context.Households.Any(hh => hh.Name == "Default")) { var hh = new Household { Name = "Default" }; context.Households.Add(hh); }
            context.SaveChanges();
            var hhId = context.Households.FirstOrDefault(h => h.Name == "Default").Id;

            ConfigurationManager.AppSettings["Categories"].Split(';').ToList().ForEach(x =>
            {
                context.Categories.Add(new Category() { Name = x, HouseholdId = hhId });
            });
            context.SaveChanges();

            if (!context.Categories.Any(ts => ts.Name == "Income")) { var cat = new Category { HouseholdId = hhId, Name = "Income" }; context.Categories.Add(cat); }
            if (!context.Categories.Any(ts => ts.Name == "Bills")) { var cat = new Category { HouseholdId = hhId, Name = "Bills" }; context.Categories.Add(cat); }
            context.SaveChanges();

            var catId = context.Categories.FirstOrDefault(c => c.Name == "Income").Id;

            if (!context.BudgetItems.Any(bi => bi.Desc == "Salary")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Salary" }; context.BudgetItems.Add(budgetItem); }
            context.SaveChanges();

            if (!context.BudgetItems.Any(bi => bi.Desc == "Other")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Other" }; context.BudgetItems.Add(budgetItem); }
            context.SaveChanges();

            catId = context.Categories.FirstOrDefault(c => c.Name == "Bills").Id;

            if (!context.BudgetItems.Any(bi => bi.Desc == "Rent")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Rent" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Phone")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Phone" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Car")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Car" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Other")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Other" }; context.BudgetItems.Add(budgetItem); }
            context.SaveChanges();


        }
    }
}
