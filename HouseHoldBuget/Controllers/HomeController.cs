using HouseHoldBuget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

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
            if (user.Household == null)
            {
                if (db.Invites.Any(h => h.InviteEmail == user.Email)) {

                    user.HouseholdId = db.Invites.FirstOrDefault(h => h.InviteEmail == user.Email).HouseholdId;  // find the hh Id of the invite
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Dashboard", "Home"); // add user to household => goto dashboard

                }
                else
                {
                    user.Household = new Household { CreatedBy = user.Email, CreateDate = System.DateTimeOffset.Now };
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Dashboard", "Home"); // add new household update user => goto dashboard
                }                                              
            }
            else
            {
                return RedirectToAction("Dashboard","Home");   // user has household => goto dashboard
            }
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        
        public ActionResult RemoveUser()
        {
            return View();
        }
        
        public ActionResult RemoveInvite(int id)
        {
            Invite invite = db.Invites.Find(id);
            db.Invites.Remove(invite);
            db.SaveChanges();
            return RedirectToAction("Households","Home");
            //return View();
        }

        [Authorize]
        public ActionResult Households()
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);

            Household household = db.Households.Find(user.HouseholdId);

            return View(household);
        }

        [Authorize]
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


//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<ActionResult> CreateComment([Bind(Include = "PostId,AuthorId,Created,Body")] Comment comment, string slug)
//{
//    if (ModelState.IsValid)
//    {
//        if (String.IsNullOrWhiteSpace(comment.Body))
//        {
//            ModelState.AddModelError("Body", "Missing Comment Text");
//            return RedirectToAction("Details", new { Slug = slug });
//        }
//        comment.Created = System.DateTimeOffset.Now;
//        comment.AuthorId = User.Identity.GetUserId();

//        db.Comments.Add(comment);
//        await db.SaveChangesAsync();
//        return RedirectToAction("Details", new { Slug = slug });

//    }
//    return RedirectToAction("Details", new { Slug = slug });
//}