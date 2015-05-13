using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseHoldBuget.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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