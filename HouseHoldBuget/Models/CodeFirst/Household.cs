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
            this.Acccounts = new HashSet<Acccount>();
            this.Categorys = new HashSet<Category>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Acccount> Acccounts { get; set; }
        public virtual ICollection<Category> Categorys { get; set; }
    }

    public class Acccount
    {
        public Acccount()
        {
            this.Transactions = new HashSet<Transaction>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }

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

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }

    public class BudgetItem
    {
        public BudgetItem()
        { }
        public int Id { get; set; }
        public string Desc { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}",
        ApplyFormatInEditMode = true)]
        public DateTimeOffset BudgetMonth { get; set; }
        public decimal BudgetAmt { get; set; }
    }
}