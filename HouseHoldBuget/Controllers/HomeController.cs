using HouseHoldBuget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using HouseHoldBuget.Helpers;
using System.Threading.Tasks;

namespace HouseHoldBuget.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);
            if (!User.Identity.IsInHousehold())
            {
                return RedirectToAction("CreateOrJoinHousehold", "Home");   // user NOT in household => goto CreateOrJoin
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");   // user has household => goto dashboard
            }
        }

        [RequireHousehold]
        public ActionResult Dashboard()
        {
            int hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            var model = new DashboardViewModel();
            var bankAccts = db.BankAccounts.Where(a => a.HouseholdId == hhId);
            foreach (var b in bankAccts)
            {
                model.bankInfo.Add(new BankAccountInfo { BankAccountId = b.Id, BankAccountName = b.Name, BankAccountBalance = Convert.ToDecimal(b.Balance) });
            }
            var trans = (from b in db.BankAccounts
                         from t in b.Transactions
                         orderby t.date descending
                         where b.HouseholdId == hhId
                         select t).Take(5).ToList();

            model.recentTransInfo = trans;

            return View(model);
        }

        [RequireHousehold]
        public JsonResult GetChartData(string barOpt)
        {
            var hhId = Int32.Parse(User.Identity.GetHouseholdId());

            var household = db.Households.Find(hhId);

            var donutData = (from c in household.Categories
                             where !c.IsIncomeCategory
                             select new
                             {
                                 label = c.Name,
                                 value = (from t in c.Transactions
                                          select t.Amt).DefaultIfEmpty().Sum()
                             }).ToList();

            var dt = System.DateTimeOffset.Now;
            var beginDate = new DateTime(dt.Year, dt.Month, 1);    /// this is barOpt1
            var endDate = beginDate.AddMonths(1).AddDays(-1);      /// this is barOpt1
                                                                   /// 
            ViewBag.selected = "opt1";

            if (barOpt == "barOpt2")
            {
                ViewBag.selected = "opt2";
                beginDate = new DateTime(dt.Year, dt.Month, 1).AddMonths(-1);
                endDate = beginDate.AddMonths(1).AddDays(-1);
            }
            else if (barOpt == "barOpt3")
            {
                ViewBag.selected = "opt3";
                beginDate = new DateTime(dt.Year, 1, 1);
                endDate = new DateTime(dt.Year, 12, 31);
            }

            var barData = (from c in household.Categories
                           select new
                           {
                               label = c.Name,
                               actual = (from t in c.Transactions
                                         where (t.date <= endDate && t.date >= beginDate)
                                         select t.Amt).DefaultIfEmpty().Sum(),
                               budget = (from b in c.BudgetItems
                                         select b.MonthlyBudgetAmt).DefaultIfEmpty().Sum()

                           }).ToList();

            return Json(new { donutData = donutData, barData = barData }, JsonRequestBehavior.AllowGet);
        }

        [RequireHousehold]
        public ActionResult RemoveInvite(int id)
        {
            Invite invite = db.Invites.Find(id);
            db.Invites.Remove(invite);
            db.SaveChanges();
            return RedirectToAction("Households", "Home");
            //return View();
        }

        [RequireHousehold]
        public ActionResult Households()
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);

            Household household = db.Households.Find(user.HouseholdId);

            return View(household);
        }

        [Authorize]
        public ActionResult CreateOrJoinHousehold()
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);

            if (user.HouseholdId != null)
            {
                RedirectToAction("Households", "Home");  // a user should not get here with a household already assigned
            }

            return View(db.Invites.Where(a => a.InviteEmail == user.Email).ToList());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrJoinHousehold(string makeHH)
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);
            user.JoinDate = System.DateTimeOffset.Now;
            user.Household = new Household { CreatedBy = user.Email, CreateDate = System.DateTimeOffset.Now };
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            await ControllerContext.HttpContext.RefreshAuthentication(user);

            BuildSampleCategories.BuildSample((int)user.HouseholdId);

            return RedirectToAction("Dashboard", "Home"); // add new household update user => goto dashboard
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AcceptInvite(int inviteId)
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);
            user.JoinDate = System.DateTimeOffset.Now;
            user.HouseholdId = db.Invites.Find(inviteId).HouseholdId;
            Invite invite = db.Invites.Find(inviteId);
            db.Invites.Remove(invite);
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            await ControllerContext.HttpContext.RefreshAuthentication(user);

            return RedirectToAction("Dashboard", "Home"); // add new household update user => goto dashboard            
        }




        [RequireHousehold]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LeaveHousehold(Household household)
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);
            user.HouseholdId = null;
            user.JoinDate = System.DateTimeOffset.Now;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            await ControllerContext.HttpContext.RefreshAuthentication(user);

            return RedirectToAction("CreateOrJoinHousehold", "Home");
        }

        [RequireHousehold]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Households([Bind(Include = "HouseholdId,InviteEmail")] Invite invite)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(invite.InviteEmail))
                {
                    ModelState.AddModelError("Invite", "Missing Email");
                    return View();
                }

                invite.InviteDate = System.DateTimeOffset.Now;

                db.Invites.Add(invite);
                db.SaveChanges();

                new EmailService().SendAsync(new IdentityMessage
               {
                   Subject = "You're invited to join... ",
                   Destination = invite.InviteEmail,
                   Body = "Please join my Household budget by going to ...",
               });
            }

            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);

            Household household = db.Households.Find(user.HouseholdId);

            return View(household);
        }

        public ActionResult About()
        {
            ViewBag.Message = "House Hold Budgeter - CoderFoundry Master Class Project";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Info:";

            return View();
        }
    }
}