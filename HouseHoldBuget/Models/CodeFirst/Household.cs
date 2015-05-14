using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseHoldBuget.Models
{
    public class Household
    {
        public Household()
        {
            this.Acccounts = new HashSet<BankAcccount>();
            this.Categories = new HashSet<Category>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BankAcccount> Acccounts { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }

    public class BankAcccount
    {
        public BankAcccount()
        {
            this.Transactions = new HashSet<Transaction>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public int HouseholdId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }

    public class Transaction
    {
        public Transaction()
        { }
        public int Id { get; set; }
        public string Desc { get; set; }
        public DateTimeOffset date { get; set; }
        public decimal Amt { get; set; }
        public decimal ReconAmt { get; set; }

        public int CategoryId { get; set; }
        public int BankAccountId { get; set; }

        public virtual Category Category { get; set; }
        public virtual BankAcccount BankAcccount { get; set; }

    }

    public class Category
    {
        public Category()
        {
            this.Transactions = new HashSet<Transaction>();
            this.BudgetItems = new HashSet<BudgetItem>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public int HouseholdId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }

    public class BudgetItem
    {
        public BudgetItem()
        { }
        public int Id { get; set; }
        public string Desc { get; set; }
        public decimal BudgetAmt { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}


//[DisplayFormat(DataFormatString = "{0:MM/yyyy}",
//ApplyFormatInEditMode = true)]
//public DateTimeOffset BudgetMonth { get; set; }