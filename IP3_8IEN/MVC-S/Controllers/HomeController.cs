using System.Web.Mvc;
using IP_8IEN.BL;
using IP_8IEN.BL.Domain.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IP_8IEN.BL.Domain.Gebruikers;
using System.IO;
using System.Web;
using IP_8IEN.BL.Domain.Dashboard;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace MVC_S.Controllers
{
    public class HomeController : Controller
    {
        private IDataManager dMgr = new DataManager();
        private IGebruikerManager gMgr = new GebruikerManager();
        private IDashManager dashMgr = new DashManager();
        private ApplicationUserManager aMgr = new ApplicationUserManager();

        public HomeController()
        {
            // initialisatie Admins zitten in InitializeAdmins()
            // initialisatie methodes zitten in Initialize()

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

        public ActionResult Index() => View();

        //Searchbar testing
        //--> Deze werkt ook (direct in de db zoeken) : er gaat enkel nog iets mis in het weergeven
        //      misschien een verkeerd gebruik van attributen
        //[HttpPost]
        //public JsonResult Index(string Prefix)
        //{
        //    //Note : you can bind same list from database  
        //    IEnumerable<Persoon> ObjList = dMgr.GetPersonen().ToList();

        //    //Searching records from list using LINQ query  
        //    var Names = (from N in ObjList
        //                 where N.Naam.StartsWith(Prefix)
        //                 select new { N.Naam });
        //    return Json(Names, JsonRequestBehavior.AllowGet);
        //}


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

        public ActionResult Dashboard() => View();

        //Get: Persoon/1
        [HttpPost]
        public ActionResult Personen(string automplete)
        {
            string naam = automplete;
            Persoon persoon = dMgr.GetPersoon(naam);
            string twit = "https://twitter.com/" + persoon.Twitter + "?ref_src=twsrc%5Etfw";
            string aantalT = "aantal tweets van " + persoon.Naam;
            ViewBag.TWITTER = twit;
            ViewBag.AANTALT = aantalT;

            ViewBag.TWITIMAGE = dMgr.GetImageString(persoon.OnderwerpId);
            ViewBag.TWITBANNER = dMgr.GetBannerString(persoon.OnderwerpId);

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

        public ActionResult Organisatie(int onderwerpId = 2) => View(dMgr.GetOrganisatie(onderwerpId));

        public ActionResult Alerts(int alertId = 1) => View(gMgr.GetAlert(alertId));

        public ActionResult WeeklyReview(int weeklyReviewId = 0)
        {
            //WeeklyReview wr = xMgr.GetWeeklyReview(weeklyReviewId);
            WeeklyReview wr = new WeeklyReview()
            {
                WeeklyReviewId = weeklyReviewId
            };
            return View(wr);
        }

        public ActionResult UserDashBoard()
        {
            //Dashbord van ingelogde gebruiker ophalen
            //Nog niet getest
            try
            {
                ApplicationUser appUser = aMgr.FindById(User.Identity.GetUserId());
                string userName = appUser.UserName;
                Gebruiker user = gMgr.FindUser(userName);

                Dashbord dashbord = dashMgr.GetDashboard(user);
                dashbord = dashMgr.UpdateDashboard(dashbord); // <-- zien dat elk DashItem minstens 3h up-to-date is

                return View(dashbord);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AdminOmgeving()
        {
            // note : deze 'if else' kun je gebruiken voor authorisatie
            if (User.IsInRole("Admin"))
            {

                return View();
            }
            else
            {
                return RedirectToAction("NotAllowed", "Error");
            }
        }

        public ActionResult Superadmin() => View();

        public ActionResult Instellingen() => View();

        [HttpGet]
        public ActionResult Zoeken()
        {
            IEnumerable<Persoon> ObjList = dMgr.GetPersonen().ToList();
            List<string> names = ObjList.Select(p => p.Naam).ToList();
            ViewData["names"] = names;
            return View();
        }

        [HttpPost]
        public ActionResult Zoeken(string search)
        {
            IEnumerable<Persoon> ObjList = dMgr.GetPersonen().Where(p => p.Naam.Contains(search));
            
            return View(ObjList);
        }

        //[HttpGet]
        //public ActionResult LijstPersonen(string search)
        //{
        //    IEnumerable<Persoon> ObjList = dMgr.GetPersonen().Where(p => p.Naam.Contains(search));
        //    return View(ObjList);
        //}

        public ActionResult InitializeAdmins()
        {
            aMgr.AddApplicationGebruikers(Path.Combine(HttpRuntime.AppDomainAppPath, "AddApplicationGebruikers.Json"));
            return View();
        }

        public ActionResult Initialize()
        {
            // Initializatie systeem //
            // >>>>>>>>> InitializeAdmins() hierboven eerst uitvoeren <<<<<<<<< //

            #region initialisatie blok databank
            dMgr.AddPersonen(Path.Combine(HttpRuntime.AppDomainAppPath, "politici.Json"));
            dMgr.ApiRequestToJson();
            //gMgr.AddAlertInstelling(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlertInstelling.json"));
            //gMgr.AddAlerts(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlerts.json"));
            #endregion
            #region test methodes
            //dMgr.AddMessages(Path.Combine(HttpRuntime.AppDomainAppPath, "textgaintest2.Json"));
            //dMgr.CountSubjMsgsPersoon();
            //dMgr.ReadOnderwerpenWithSubjMsgs();
            //dMgr.GetAlerts();
            //gMgr.AddGebruikers(Path.Combine(HttpRuntime.AppDomainAppPath, "AddGebruikersInit.Json"));
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
        public ActionResult GetRank(int aantal) => Json(dMgr.GetRanking(aantal,100), JsonRequestBehavior.AllowGet);

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