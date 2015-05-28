using HouseHoldBuget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HouseHoldBuget.Helpers
{
    public static class BuildSampleCategories
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void BuildSample(int hhId)
        {
            // Categories // Categories // Categories // Categories // Categories // Categories // Categories // Categories //
            // Categories // Categories // Categories // Categories // Categories // Categories // Categories // Categories //
            db.Categories.Add(new Category { HouseholdId = hhId, IsIncomeCategory = true, Name = "Income", });

            db.Categories.Add(new Category { HouseholdId = hhId, IsIncomeCategory = false, Name = "Housing" });
            db.Categories.Add(new Category { HouseholdId = hhId, IsIncomeCategory = false, Name = "Utilities" });
            db.Categories.Add(new Category { HouseholdId = hhId, IsIncomeCategory = false, Name = "Health/Medical" });
            db.Categories.Add(new Category { HouseholdId = hhId, IsIncomeCategory = false, Name = "Transportation" });
            db.Categories.Add(new Category { HouseholdId = hhId, IsIncomeCategory = false, Name = "Credit Cards / Loans" });
            db.Categories.Add(new Category { HouseholdId = hhId, IsIncomeCategory = false, Name = "Food/Entertainment" });
            db.Categories.Add(new Category { HouseholdId = hhId, IsIncomeCategory = false, Name = "Children" });
            db.Categories.Add(new Category { HouseholdId = hhId, IsIncomeCategory = false, Name = "Personal" });
            db.Categories.Add(new Category { HouseholdId = hhId, IsIncomeCategory = false, Name = "Savings" });

            db.SaveChanges();


            // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems //
            // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems // BudgetItems //

            int catId = (from c in db.Categories
                         where c.HouseholdId == hhId && c.Name == "Income"
                         select c.Id).Single();

            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Monthly Pay (after Taxes)" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Alimony/Child Support" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Other Income" });
            db.SaveChanges();


            catId = (from c in db.Categories
                     where c.HouseholdId == hhId && c.Name == "Housing"
                     select c.Id).Single();
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Mortgage or Rent" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Real Estate Property Tax" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Personal property tax" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Homeowner or Renters Insurance" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "HOA fees" });

            db.SaveChanges();

            catId = (from c in db.Categories
                     where c.HouseholdId == hhId && c.Name == "Utilities"
                     select c.Id).Single();
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Electric" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Gas/Heating Oil" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Water/Sewage" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Telephone" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Trash Collection" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Cable TV" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Internet Provider" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Cell Phone" });
            db.SaveChanges();

            catId = (from c in db.Categories
                     where c.HouseholdId == hhId && c.Name == "Health/Medical"
                     select c.Id).Single();
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Medical Insurance" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Dental Insurance" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Doctor" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Dentist" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Orthodontist" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Therapist" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Eyeglasses/ophthalmologist" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Hospital/emergency" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Medicines" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Other Medical" });
            db.SaveChanges();

            catId = (from c in db.Categories
                     where c.HouseholdId == hhId && c.Name == "Transportation"
                     select c.Id).Single();
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Car payments" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Car insurance" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Car maintenance/repair" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Mass transit costs" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Gasoline" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Parking/Tolls" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Tags/Inspection" });
            db.SaveChanges();

            catId = (from c in db.Categories
                     where c.HouseholdId == hhId && c.Name == "Credit Cards / Loans"
                     select c.Id).Single();
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Card balance" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Student Loan" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Legal Fees" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Alimony/child support paid" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "zzz" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "zzz" });
            db.SaveChanges();

            catId = (from c in db.Categories
                     where c.HouseholdId == hhId && c.Name == "Food/Entertainment"
                     select c.Id).Single();
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Groceries" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Meals out" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Entertainment (movies, etc.)" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Hobbies" });
            db.SaveChanges();

            catId = (from c in db.Categories
                     where c.HouseholdId == hhId && c.Name == "Children"
                     select c.Id).Single();
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Child care" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "School tuition" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Lunch money" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "School supplies" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Lessons/sports" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "New clothing" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Haircuts" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Allowances" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Other Child Expense" });
            db.SaveChanges();

            catId = (from c in db.Categories
                     where c.HouseholdId == hhId && c.Name == "Personal"
                     select c.Id).Single();
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Dry cleaning/laundry" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Personal grooming" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Wardrobe" });
            db.SaveChanges();

            catId = (from c in db.Categories
                     where c.HouseholdId == hhId && c.Name == "Savings"
                     select c.Id).Single();
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Monthly Savings" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Gifts" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "House Maintenance/Repairs" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Furniture" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Church/Charity" });
            db.BudgetItems.Add(new BudgetItem { CategoryId = catId, Desc = "Vacation" });
            db.SaveChanges();


            db.BankAccounts.Add(new BankAccount { HouseholdId = hhId, Name = "Checking", Balance = 0M, ReconciledBalance = 0M });
            db.SaveChanges();
            var bankIdId = (from ba in db.BankAccounts
                            where ba.HouseholdId == hhId && ba.Name == "Checking"
                            select ba.Id).Single();
            db.Transactions.Add(new Transaction { CategoryId = catId, Desc = "Beginning Balance", Amt = 0M, BankAccountId = bankIdId, date = System.DateTimeOffset.Now });

            db.SaveChanges();


        }
    }
}

