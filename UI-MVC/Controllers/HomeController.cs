using IP_8IEN.UI_MVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IP_8IEN.UI_MVC.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            CookieHandler();
            return View();
        }

        public ActionResult About()
        {
            CookieHandler();
            ViewBag.Message = Resources.Resources.AboutMessage;

            return View();
        }

        public ActionResult Contact()
        {
            CookieHandler();
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}