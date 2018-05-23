using IP_8IEN.BL;
using IP_8IEN.BL.Domain.Gebruikers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IP_8IEN.UI.MVC_S.Controllers
{
    public class AlertController : Controller
    {
        private IGebruikerManager mgr = new GebruikerManager();

        // 2 apr 2018 : Stephane
        // GET: Alert
        public ActionResult Index()
        {
            IEnumerable<Alert> alerts = mgr.GetAlerts();
            return View(alerts);
        }
        
        // GET: Alert/Details/5
        public ActionResult Details(int id) => View();

        // GET: Alert/Create
        public ActionResult Create() => View();

        // POST: Alert/Create
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

        // GET: Alert/Edit/5
        public ActionResult Edit(int id) => View();

        // POST: Alert/Edit/5
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

        // GET: Alert/Delete/5
        public ActionResult Delete(int id) => View();

        // POST: Alert/Delete/5
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
