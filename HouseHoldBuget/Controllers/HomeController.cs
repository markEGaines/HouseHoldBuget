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
            return View();
        }

        // GET: BankAccounts
        [RequireHousehold]
        public ActionResult DashboardBankAccounts()
        {
            int hhId = Convert.ToInt32(User.Identity.GetHouseholdId());
            return PartialView(db.BankAccounts.Where(a => a.HouseholdId == hhId));
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
         //   Household household = db.Households.Find(User.Identity.GetHouseholdId());

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