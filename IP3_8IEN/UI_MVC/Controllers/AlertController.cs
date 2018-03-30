using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IP3_8IEN.BL;
using IP3_8IEN.BL.Domain.Gebruikers;

namespace UI_MVC3.Controllers
{
    public class AlertController : Controller
    {
        // GET: Alerts
        public ActionResult Index()
        {
            GebruikerManager gmgr = new GebruikerManager();
            IEnumerable<Alert> allAlerts = gmgr.GetAlerts();
            return View(allAlerts);
        }

        // GET: Alerts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Alerts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Alerts/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Alerts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Alerts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Alerts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Alerts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}