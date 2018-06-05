using IP3_8IEN.BL;
using IP3_8IEN.BL.Domain.Gebruikers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVC_S.Controllers
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
        public ActionResult Details()
        {
            return View();
        }

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

        //Lijst met PositiefNegatiefs alertinstellingen
        public ActionResult ListPositiefNegatief()
        {
            List<PositiefNegatief> pns = mgr.GetPositiefNegatiefsByUser();
            return PartialView(pns);
        }

        //Lijst met HogerLagers alertinstellingen
        public ActionResult ListHogerLager()
        {
            List<HogerLager> hls = mgr.GetHogerLagersByUser();
            return PartialView(hls);
        }

        //Lijst met PositiefNegatiefs alertinstellingen
        public ActionResult ListValueFluctuations()
        {
            List<ValueFluctuation> vfs = mgr.GetValueFluctuationsByUser();
            return PartialView(vfs);
        }

        //Lijst met Alerts ophalen via JSON
        public ActionResult AlertsByName(string name)
        {
            //Dashbord van ingelogde gebruiker ophalen
            try
            {
                //ApplicationUser appUser = aMgr.FindById(User.Identity.GetUserId());
                //string userName = appUser.UserName;
                Gebruiker user = mgr.FindUser(name);

                List<Alert> alerts = mgr.GetAlerts().ToList();

                var list = JsonConvert.SerializeObject(alerts, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return Content(list, "application/json");
            }
            catch (Exception ex)
            {
                return Json("error" + ex, JsonRequestBehavior.AllowGet);

            }
        }
    }
}
