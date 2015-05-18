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
            this.Accounts = new HashSet<BankAcccount>();
            this.Categories = new HashSet<Category>();
            this.Users = new HashSet<ApplicationUser>();
            this.Invites = new HashSet<Invite>();
        }
        public int Id { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? UpdateDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<BankAcccount> Accounts { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
    }

    public class Invite
    {
        public Invite()
        { }
        public int Id { get; set; }
        public string InviteEmail { get; set; }
        public DateTimeOffset? InviteDate { get; set; }
        public string Challenge { get; set; }

        public int HouseholdId { get; set; }        
    }

    public class BankAcccount
    {
        public BankAcccount()
        {
            this.Transactions = new HashSet<Transaction>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Balance { get; set; }

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