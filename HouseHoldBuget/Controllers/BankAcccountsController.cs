﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseHoldBuget.Models;

namespace HouseHoldBuget.Controllers
{
    public class BankAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: BankAccounts
        [Authorize]
        public ActionResult Index()
        {
            var hhId = db.Households.FirstOrDefault(h => h.CreatedBy == User.Identity.Name).Id;

            return View(db.Accounts.Where(a => a.HouseholdId == hhId));
            
         //   return View(db.Accounts.ToList());
        }

        // GET: BankAccounts/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAcccount bankAcccount = db.Accounts.Find(id);
            if (bankAcccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAcccount);
        }

        // GET: BankAccounts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Balance,HouseholdId")] BankAcccount bankAcccount)
        {
            if (ModelState.IsValid)
            {
                bankAcccount.HouseholdId = db.Households.FirstOrDefault(h => h.CreatedBy == User.Identity.Name).Id;

                db.Accounts.Add(bankAcccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bankAcccount);
        }

        // GET: BankAccounts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAcccount bankAcccount = db.Accounts.Find(id);
            if (bankAcccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAcccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Balance,HouseholdId")] BankAcccount bankAcccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankAcccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bankAcccount);
        }

        // GET: BankAccounts/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAcccount bankAcccount = db.Accounts.Find(id);
            if (bankAcccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAcccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAcccount bankAcccount = db.Accounts.Find(id);
            db.Accounts.Remove(bankAcccount);
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