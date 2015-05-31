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

namespace HouseHoldBuget.Controllers
{
    public class BudgetItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BudgetItems
        [RequireHousehold]
        public async Task<ActionResult> Index()
        {
            int hhId = Convert.ToInt32(User.Identity.GetHouseholdId());

            var budgetItems = (from c in db.Categories
                               from b in c.BudgetItems
                               where c.HouseholdId == hhId
                               select b);

            return View(budgetItems.ToList());

        }

        // GET: BudgetItems/Details/5
        [RequireHousehold]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = await db.BudgetItems.FindAsync(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetItem);
        }

        // GET: BudgetItems/Create
        [RequireHousehold]
        public ActionResult Create()
        {
            int hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            var cats = (from c in db.Categories where c.HouseholdId == hhId select c);
            ViewBag.CategoryId = new SelectList(cats, "Id", "Name");
            return View();
        }

        // POST: BudgetItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequireHousehold]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Desc,BudgetAmt,AnnualFreq,MonthlyBudgetAmt,CategoryId")] BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {
                db.BudgetItems.Add(budgetItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            int hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            var cats = (from c in db.Categories where c.HouseholdId == hhId select c);
            ViewBag.CategoryId = new SelectList(cats, "Id", "Name", budgetItem.CategoryId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Edit/5
        [RequireHousehold]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = await db.BudgetItems.FindAsync(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            int hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            var cats = (from c in db.Categories where c.HouseholdId == hhId select c);
            ViewBag.CategoryId = new SelectList(cats, "Id", "Name", budgetItem.CategoryId);
            return View(budgetItem);
        }

        // POST: BudgetItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequireHousehold]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Desc,BudgetAmt,AnnualFreq,MonthlyBudgetAmt,CategoryId")] BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            int hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            var cats = (from c in db.Categories where c.HouseholdId == hhId select c);
            ViewBag.CategoryId = new SelectList(cats, "Id", "Name", budgetItem.CategoryId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Delete/5
        [RequireHousehold]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = await db.BudgetItems.FindAsync(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetItem);
        }

        // POST: BudgetItems/Delete/5
        [RequireHousehold]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BudgetItem budgetItem = await db.BudgetItems.FindAsync(id);
            db.BudgetItems.Remove(budgetItem);
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
