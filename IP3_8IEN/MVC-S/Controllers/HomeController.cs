using System.Web.Mvc;
using IP_8IEN.BL;
using IP_8IEN.BL.Domain.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using IP_8IEN.BL.Domain.Gebruikers;
using System.Web.Hosting;
using System.IO;
using System.Web;

namespace MVC_S.Controllers
{
    public class HomeController : Controller
    {
        private IDataManager dMgr;
        private IGebruikerManager gMgr;

        public HomeController()
        {
            // Hier wordt voorlopig wat testdata doorgegeven aan de 'Managers'
            // Let op: telkens de 'HomeController() aangesproken wordt worden er methodes uitgevoerd
            dMgr = new DataManager();
            gMgr = new GebruikerManager();
            
            #region initialisatie blok databank
            dMgr.AddPersonen(Path.Combine(HttpRuntime.AppDomainAppPath, "politici.Json"));
            dMgr.ApiRequestToJson();

            gMgr.AddGebruikers(Path.Combine(HttpRuntime.AppDomainAppPath, "AddGebruikersInit.Json"));
            gMgr.AddAlertInstelling(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlertInstelling.json"));
            gMgr.AddAlerts(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlerts.json"));
            #endregion

            //**** dit zijn test methodes ****//
            //dMgr.CountSubjMsgsPersoon();
            //dMgr.ReadOnderwerpenWithSubjMsgs();
            //dMgr.GetAlerts();
            //**** dit zijn test methodes ****//

            HostingEnvironment.QueueBackgroundWorkItem(ct => WeeklyReview(gMgr));
            //HostingEnvironment.QueueBackgroundWorkItem(ct => RetrieveAPIData(dMgr));

        }
        private async Task RetrieveAPIData(IDataManager dMgr)
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    dMgr.ApiRequestToJson();
                });
                Thread.Sleep(10800000);
            }
        }

        private async Task WeeklyReview(IGebruikerManager gMgr)
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    gMgr.WeeklyReview();
                });
                Thread.Sleep(10800000);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Dashboard()
        {
            
            return View();
        }

        //Get: Persoon/1
        public ActionResult Personen(/*int onderwerpId*/)
        {
            int id = 1;
            Persoon persoon = dMgr.GetPersoon(id);
            
            return View(persoon);
        }

        public ActionResult Themas(/*int onderwerpId*/)
        {
            //Thema thema = xMgr.GetThema(onderwerpId);
            /*  verwijder alle onderstaande code buiten de return
             *      zodra er via bovenstaande methode
             *      een thema kan binnengehaald worden
             *      en vervang de xMgr met de correcte mgr*/
            Thema thema = new Thema()
            {
                ThemaString = "thema",
                Beschrijving = "beschrijving over het thema"
            };
            return View(thema);
        }

        public ActionResult Organisatie(/*int onderwerpId*/)
        {
            int id = 2;
            Organisatie organisatie = dMgr.GetOrganisatie(id);

            return View(organisatie);
        }

        public ActionResult Alerts(/*int alertId*/)
        {
            int id = 1;
            Alert alert = gMgr.GetAlert(id);
            return View(alert);
        }

        public ActionResult WeeklyReview(int weeklyReviewId)
        {
            //WeeklyReview wr = xMgr.GetWeeklyReview(weeklyReviewId);
            WeeklyReview wr = new WeeklyReview();
            return View(wr);
        }

        public ActionResult AdminCRUD()
        {

            return View();
        }

        public ActionResult AdminOmgeving()
        {
            // stephane : note : deze 'if else' kun je gebruiken voor authorisatie
            if (User.IsInRole("Admin")){

                return View();
            } else
            {
                return RedirectToAction("NotAllowed", "Error");
            }
        }

        public ActionResult Superadmin()
        {

            return View();
        }

        public ActionResult Instellingen()
        {

            return View();
        }

        public ActionResult Zoeken()
        {

            return View();
        }
    }
}