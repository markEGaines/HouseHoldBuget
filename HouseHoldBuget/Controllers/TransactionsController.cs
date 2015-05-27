using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseHoldBuget.Models;
using HouseHoldBuget.Helpers;
using Microsoft.AspNet.Identity;

namespace HouseHoldBuget.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        [RequireHousehold]
        [Route("Accounts/{accountId:int?}")]
        public async Task<ActionResult> Index(int? accountId)
        {

            int hhId = Convert.ToInt32(User.Identity.GetHouseholdId());

            var house = db.Households.Include(h => h.Accounts).Include(a => a.Accounts.Select(t => t.Transactions)).FirstOrDefault(hh => hh.Id == hhId);



            if (accountId == null)
            {
                var first = house.Accounts.FirstOrDefault();
                if (first != null)
                    accountId = first.Id;
            }

            ViewBag.AccountId = new SelectList(house.Accounts.ToList(), "Id", "Name", accountId);
            ViewBag.bankAccountId = accountId;

            ViewBag.balance = db.BankAccounts.Find(accountId).Balance;
            ViewBag.reconciledBalance = db.BankAccounts.Find(accountId).ReconciledBalance;

            if (accountId != null)
                return View(house.Accounts.First(a => a.Id == accountId).Transactions.ToList());
            return View(new List<Transaction>());
        }

        // GET: Transactions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create(int? accountId)
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.bankAccountId = accountId;
            Transaction trx = new Transaction();
            trx.BankAccountId = Convert.ToInt32(accountId);
            return View(trx);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,date,Desc,CategoryId,Amt,ReconAmt,UpdatedBy,BankAccountId,CreateDate,CreatedBy,UpdateDate")] Transaction transaction)
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);

            transaction.CreateDate = System.DateTimeOffset.Now;
            transaction.CreatedBy = user.Email;
            transaction.ReconAmt = 0;

            var bankAccount = (from a in db.BankAccounts
                               where a.Id == transaction.BankAccountId
                               select a).FirstOrDefault();

            if(bankAccount == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {

                bankAccount.Balance += transaction.Amt;

                db.Transactions.Add(transaction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,date,Desc,CategoryId,Amt,ReconAmt,UpdatedBy,BankAccountId,CreateDate,CreatedBy,UpdateDate")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                var user = db.Users.Find(userid);

                transaction.UpdatedBy = user.Email;
                transaction.UpdateDate = System.DateTimeOffset.Now;

                db.Entry(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Transaction transaction = await db.Transactions.FindAsync(id);
            db.Transactions.Remove(transaction);
            await db.SaveChangesAsync();
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
