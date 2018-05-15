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
using System.Web.Helpers;
using IP3_8IEN.BL.Domain.Dashboard;
using IP_8IEN.BL.Domain.Gebruikers;
using Microsoft.Ajax.Utilities;

namespace MVC_S.Controllers
{
    public class HomeController : Controller
    {
        private IDataManager dMgr;
        private IGebruikerManager gMgr;
        private ApplicationUserManager aMgr;

        public HomeController()
        {
            // Hier wordt voorlopig wat testdata doorgegeven aan de 'Managers'
            // Let op: telkens de 'HomeController() aangesproken wordt worden er methodes uitgevoerd
            dMgr = new DataManager();
            gMgr = new GebruikerManager();

            aMgr = new ApplicationUserManager();

            #region initialisatie blok databank
            //dMgr.AddPersonen(Path.Combine(HttpRuntime.AppDomainAppPath, "politici.Json"));
            //dMgr.ApiRequestToJson();
            //gMgr.AddAlertInstelling(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlertInstelling.json"));
            //gMgr.AddAlerts(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlerts.json"));
            //aMgr.AddApplicationGebruikers(Path.Combine(HttpRuntime.AppDomainAppPath, "AddApplicationGebruikers.Json"));

            #endregion

            //**** dit zijn test methodes ****//
            //dMgr.CountSubjMsgsPersoon();
            //dMgr.ReadOnderwerpenWithSubjMsgs();
            //dMgr.GetAlerts();
            //gMgr.AddGebruikers(Path.Combine(HttpRuntime.AppDomainAppPath, "AddGebruikersInit.Json"));
            //**** dit zijn test methodes ****//

            //HostingEnvironment.QueueBackgroundWorkItem(ct => WeeklyReview(gMgr));
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
            string twit = "https://twitter.com/" + persoon.Twitter + "?ref_src=twsrc%5Etfw";
            string aantalT = "aantal tweets van " + persoon.Naam;
            ViewBag.TWITTER = twit;
            ViewBag.AANTALT = aantalT;

            ViewBag.TWITIMAGE = dMgr.GetImageString(id);
            ViewBag.TWITBANNER = dMgr.GetBannerString(id);

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

        // GET : Home/Create
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

        public ActionResult Initialize()
        {
            // Hier wordt voorlopig wat testdata doorgegeven aan de 'Managers'
            // Let op: telkens de 'HomeController() aangesproken wordt worden er methodes uitgevoerd
            dMgr = new DataManager();
            gMgr = new GebruikerManager();

            #region initialisatie blok databank
            //dMgr.AddPersonen(Path.Combine(HttpRuntime.AppDomainAppPath, "politici.Json"));
            //dMgr.ApiRequestToJson();
            gMgr.AddGebruikers(Path.Combine(HttpRuntime.AppDomainAppPath, "AddGebruikersInit.Json"));
            //gMgr.AddAlertInstelling(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlertInstelling.json"));
            //gMgr.AddAlerts(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlerts.json"));
            #endregion


            

            return View();
        }
        public ActionResult Grafiektest2()
        {
            Persoon persoon = dMgr.GetPersoon(170);
            int aantalTweets = dMgr.GetNumber(persoon);
           // int aantalTweets = 69;
            ViewBag.NUMMER1 = aantalTweets;
            ViewBag.naam1 = persoon.Naam;
            //System.Diagnostics.Debug.WriteLine("tweets per dag"+aantalTweets);

            return View();
        }

        public ActionResult GetData(int id)
        {
            Persoon persoon = dMgr.GetPersoon(id);
            return Json(dMgr.GetTweetsPerDag(persoon,20), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRank(int aantal)
        {
            
            return Json(dMgr.GetRanking(aantal,100), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetData2(int id1, int id2, int id3, int id4, int id5 )
        {
            Persoon persoon1 = dMgr.GetPersoon(id1);
            Persoon persoon2 = dMgr.GetPersoon(id2);
            Persoon persoon3 = dMgr.GetPersoon(id3);
            Persoon persoon4 = dMgr.GetPersoon(id4);
            Persoon persoon5 = dMgr.GetPersoon(id5);
            return Json(dMgr.GetTweetsPerDag2(persoon1, persoon2, persoon3, persoon4, persoon5, 20), JsonRequestBehavior.AllowGet);
        }




    }
}