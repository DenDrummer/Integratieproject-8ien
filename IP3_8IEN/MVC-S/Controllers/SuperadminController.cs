using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_S.Controllers
{
    public class SuperadminController : Controller
    {
        // GET: Superadmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Adminlijst()
        {
            return View();
        }

        public ActionResult Inloggen()
        {
            return View();
        }

        public ActionResult SMBeheren()
        {
            return View();
        }
    } 

}