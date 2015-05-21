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
                    Household = new Household { CreatedBy = "markegaines@gmail.com" , CreateDate = System.DateTimeOffset.Now },
                    JoinDate = System.DateTimeOffset.Now,
                    UserName = "markegaines@gmail.com",
                    Email = "markegaines@gmail.com",
                    EmailConfirmed = true,
                }, "Plugh4!");
            }

            if (!context.Households.Any(hh => hh.CreatedBy == "markegaines@gmail.com")) { var hh = new Household { CreatedBy = "markegaines@gmail.com" }; context.Households.Add(hh); }
            context.SaveChanges();
            var hhId = context.Households.FirstOrDefault(h => h.CreatedBy == "markegaines@gmail.com").Id;

            if (!context.Users.Any(r => r.Email == "donaldfgaines@gmail.com"))
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                userManager.Create(new ApplicationUser
                {
                    HouseholdId = hhId,
                    JoinDate = System.DateTimeOffset.Now,
                    UserName = "donaldfgaines@gmail.com",
                    Email = "donaldfgaines@gmail.com",
                    EmailConfirmed = true,
                }, "Plugh4!");
            }


            //ConfigurationManager.AppSettings["Categories"].Split(';').ToList().ForEach(x =>
            //{
            //    context.Categories.Add(new Category() { Name = x, HouseholdId = hhId });
            //});
            //context.SaveChanges();
     
            // Categories // Categories // Categories // Categories // Categories // Categories // Categories // Categories //
            // Categories // Categories // Categories // Categories // Categories // Categories // Categories // Categories //
            if (!context.Categories.Any(ts => ts.Name == "Income")) { var cat = new Category { HouseholdId = hhId, Name = "Income" }; context.Categories.Add(cat); }
            if (!context.Categories.Any(ts => ts.Name == "Expenses")) { var cat = new Category { HouseholdId = hhId, Name = "Expenses" }; context.Categories.Add(cat); }
            context.SaveChanges();


            // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems //
            // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems //

            var catId = context.Categories.FirstOrDefault(c => c.Name == "Income").Id;
            if (!context.BudgetItems.Any(bi => bi.Desc == "Salary")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Salary" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Other")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Other" }; context.BudgetItems.Add(budgetItem); }
            context.SaveChanges();

            catId = context.Categories.FirstOrDefault(c => c.Name == "Expenses").Id;
            if (!context.BudgetItems.Any(bi => bi.Desc == "Housing")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Housing" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Utilities")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Utilities" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Health/Medical")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Health/Medical" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Transportation")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Transportation" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Other")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Other" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Food/Entertainment")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Food/Entertainment" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Children")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Children" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Personal")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Personal" }; context.BudgetItems.Add(budgetItem); }
            if (!context.BudgetItems.Any(bi => bi.Desc == "Savings")) { var budgetItem = new BudgetItem { CategoryId = catId, Desc = "Savings" }; context.BudgetItems.Add(budgetItem); }
            context.SaveChanges();

            var trans = new Transaction { CategoryId = catId, Desc = "Something Silly" , Amt=100.01M, BankAccountId=1, date=System.DateTimeOffset.Parse("4/1/2015")}; context.Transactions.Add(trans);

            trans = new Transaction { CategoryId = catId, Desc = "Something Even Worse", Amt = 200.01M, BankAccountId = 1, date = System.DateTimeOffset.Parse("4/2/2015") }; context.Transactions.Add(trans); 
            trans = new Transaction { CategoryId = catId, Desc = "Car Payment", Amt = 1250.51M, BankAccountId = 1, date = System.DateTimeOffset.Parse("4/2/2015") }; context.Transactions.Add(trans);
            
            
            context.SaveChanges();
        
        }
    }
}