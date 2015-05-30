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
        public async Task<ActionResult> IndexDetails(int? id)
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
            return PartialView(transaction);
        }

        // GET: Transactions/IndexCreate
        public ActionResult IndexCreate(int? accountId)
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.bankAccountId = accountId;
            Transaction trx = new Transaction();
            trx.BankAccountId = Convert.ToInt32(accountId);
            trx.date = System.DateTimeOffset.Now;
            return PartialView(trx);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IndexCreate([Bind(Include = "Id,date,Desc,CategoryId,Amt,ReconAmt,UpdatedBy,BankAccountId,CreateDate,CreatedBy,UpdateDate")] Transaction transaction)
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);

            transaction.CreateDate = System.DateTimeOffset.Now;
            transaction.CreatedBy = user.Email;
            transaction.UpdateDate = System.DateTimeOffset.Now;
            transaction.UpdatedBy = user.Email;
            transaction.ReconAmt = 0;

            var bankAccount = (from a in db.BankAccounts
                               where a.Id == transaction.BankAccountId
                               select a).FirstOrDefault();

            if (bankAccount == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                bankAccount.Balance += transaction.Amt;

                db.Transactions.Add(transaction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { accountId = transaction.BankAccountId });
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/IndexEdit/5
        public async Task<ActionResult> IndexEdit(int? id)
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
            ViewBag.EditCreateTitle = "Edit";

            return PartialView(transaction);
        }

        // POST: Transactions/IndexEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IndexEdit([Bind(Include = "Id,date,Desc,CategoryId,Amt,ReconAmt,UpdatedBy,BankAccountId,CreateDate,CreatedBy,UpdateDate")] Transaction transaction)
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);

            var bankAccount = (from a in db.BankAccounts
                               where a.Id == transaction.BankAccountId
                               select a).FirstOrDefault();

            if (bankAccount == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delta = transaction.Amt - (from t in db.Transactions.AsNoTracking()
                                           where t.Id == transaction.Id
                                           select t).FirstOrDefault().Amt;

            var deltaRecon = transaction.ReconAmt - (from t in db.Transactions.AsNoTracking()
                                           where t.Id == transaction.Id
                                           select t).FirstOrDefault().ReconAmt;

            transaction.UpdatedBy = user.Email;
            transaction.UpdateDate = System.DateTimeOffset.Now;

            if (ModelState.IsValid)
            {
                bankAccount.Balance += delta;
                bankAccount.ReconciledBalance += deltaRecon;

                db.Entry(transaction).State = EntityState.Modified;
                db.Entry(bankAccount).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { accountId = transaction.BankAccountId });
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<ActionResult> IndexDelete(int? id)
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
            return PartialView(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("IndexDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Transaction transaction = await db.Transactions.FindAsync(id);

            var bankAccount = (from a in db.BankAccounts
                               where a.Id == transaction.BankAccountId
                               select a).FirstOrDefault();

            if (bankAccount == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            bankAccount.Balance -= transaction.Amt;
            bankAccount.ReconciledBalance -= transaction.ReconAmt;


            db.Entry(bankAccount).State = EntityState.Modified;
            db.Transactions.Remove(transaction);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { accountId = transaction.BankAccountId });
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
