using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseHoldBuget.Models;
using HouseHoldBuget.Helpers;

namespace HouseHoldBuget.Controllers
{
    public class BankAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: BankAccounts
        [RequireHousehold]
        public ActionResult Index()
        {
           
            int hhId = Convert.ToInt32(User.Identity.GetHouseholdId());

            return View(db.BankAccounts.Where(a => a.HouseholdId == hhId));
            
         //   return View(db.Accounts.ToList());
        }

        // GET: BankAccounts/Details/5
        [RequireHousehold]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        [RequireHousehold]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Balance,HouseholdId")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                bankAccount.HouseholdId = Convert.ToInt32(User.Identity.GetHouseholdId());

                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
        [RequireHousehold]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RequireHousehold]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Balance,ReconciledBalance")] BankAccount bankAccount)
        {
            bankAccount.HouseholdId =  Convert.ToInt32(User.Identity.GetHouseholdId());

            if (ModelState.IsValid)
            {
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bankAccount);
        }


        // POST: BankAccounts/BalanceReCalc/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RequireHousehold]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BalanceReCalc([Bind(Include = "Id,Name,Balance,ReconciledBalance")] BankAccount bankAccount)
        {
            bankAccount.HouseholdId = Convert.ToInt32(User.Identity.GetHouseholdId());

            var trans = from t in db.Transactions
                        where t.BankAccountId == bankAccount.Id
                        select new { t.Amt, t.ReconAmt };

            bankAccount.Balance = trans.ToList().Select(s => s.Amt).Sum();
            bankAccount.ReconciledBalance = trans.ToList().Select(s => s.ReconAmt).Sum();                          

            if (ModelState.IsValid)
            {
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "BankAccounts", new { bankAccount.Id });
            }
            return RedirectToAction("Edit", "BankAccounts", new { bankAccount.Id });
        }

        // GET: BankAccounts/Delete/5
        [RequireHousehold]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [RequireHousehold]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BankAccounts.Find(id);
            db.BankAccounts.Remove(bankAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
