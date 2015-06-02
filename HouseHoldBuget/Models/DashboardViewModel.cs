using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseHoldBuget.Models
{
    public class BankAccountInfo
    {
        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; }
        public decimal BankAccountBalance { get; set; }
    }

    public class RecentTransactions
    {
        public int TransactionId { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTimeOffset TransactionDate { get; set; }
        public string TransactionDesc { get; set; }
        public decimal TransactionAmt { get; set; }
        public int TransactionAccountId { get; set; }
        public string TransactionAccountName { get; set; }
    }

    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            bankInfo = new List<BankAccountInfo>();
            recentTransInfo = new List<Transaction>();
        }
        public List<BankAccountInfo> bankInfo { get; set; }
        public List<Transaction> recentTransInfo { get; set; }
    }
}