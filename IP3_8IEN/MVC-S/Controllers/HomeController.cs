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
using System.Web.Helpers;
using IP_8IEN.BL.Domain.Dashboard;
using Microsoft.Ajax.Utilities;

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

            //string id = System.DateTime.Now.ToString();
            //gMgr.AddGebruiker("testuser", id, "dummy", "plug");

            //HostingEnvironment.QueueBackgroundWorkItem(ct => WeeklyReview(gMgr));
            //HostingEnvironment.QueueBackgroundWorkItem(ct => RetrieveAPIData(dMgr));
            gMgr.GetAlertHogerLagers();
            gMgr.GetAlertPositiefNegatiefs();
            gMgr.GetAlertValueFluctuations();
            
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

            string screenname = persoon.Twitter;
            ViewBag.TWITIMAGE = dMgr.GetImageString(screenname);
            ViewBag.TWITBANNER = dMgr.GetBannerString(screenname);

            return View(persoon);
        }
        public ActionResult Personen(int onderwerpId = 1)
        {
            Persoon persoon = dMgr.GetPersoon(onderwerpId);
            string twit = "https://twitter.com/" + persoon.Twitter + "?ref_src=twsrc%5Etfw";
            string aantalT = "aantal tweets van " + persoon.Naam;
            ViewBag.TWITTER = twit;
            ViewBag.AANTALT = aantalT;

            ViewBag.TWITIMAGE = dMgr.GetImageString(persoon.Twitter);
            ViewBag.TWITBANNER = dMgr.GetBannerString(persoon.Twitter);

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
                Naam = "thema",
                Beschrijving = "beschrijving over het thema"
            };
            return View(thema);
        }

        public ActionResult Organisatie(int onderwerpId = 22) {
            string screenname = dMgr.GetOrganisatie(onderwerpId).Twitter;
            System.Diagnostics.Debug.WriteLine("Screenname: " + screenname);
            ViewBag.TWITIMAGE = dMgr.GetImageString(screenname);
            ViewBag.TWITBANNER = dMgr.GetBannerString(screenname);
            return View(dMgr.GetOrganisatie(onderwerpId));
        }


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
            try
            {
                ApplicationUser appUser = aMgr.FindById(User.Identity.GetUserId());
                string userName = appUser.UserName;
                Gebruiker user = gMgr.FindUser(userName);

                Dashbord dashbord = dashMgr.GetDashboardWithFollows(user);
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
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("NotAllowed", "Error");
            }
        }

        public ActionResult Superadmin() => View();

        public ActionResult Instellingen() => View();

        public ActionResult LijstPersonen() => View(dMgr.GetPersonen());

        public ActionResult LijstThemas() => View(new List<Thema>()
            {
                new Thema()
                {
                    OnderwerpId = 285,
                    Naam = "ukip",
                    Hashtags = new List<string>()
                    {
                        "ukip"
                    }
                }
            });

        public ActionResult LijstOrganisaties() => View(dMgr.GetOrganisaties());

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
            //int aantalTweets = 69;
            ViewBag.NUMMER1 = aantalTweets;
            ViewBag.naam1 = persoon.Naam;
            //System.Diagnostics.Debug.WriteLine("tweets per dag"+aantalTweets);
            int[] init = {0, 1, 3, 2, 8, 6, 5, 4, 9, 7 };
            //ViewData["init"] = init;
            ViewBag.INIT = init;
            return View();
        }

        public ActionResult GetData(int id)
        {
            Persoon persoon = dMgr.GetPersoon(id);
            return Json(dMgr.GetTweetsPerDag(persoon, 20), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRank(int aantal) => Json(dMgr.GetRanking(aantal, 100), JsonRequestBehavior.AllowGet);

        public ActionResult GetData2(int id1, int id2, int id3, int id4, int id5)
        {
            Persoon persoon1 = dMgr.GetPersoon(id1);
            Persoon persoon2 = dMgr.GetPersoon(id2);
            Persoon persoon3 = dMgr.GetPersoon(id3);
            Persoon persoon4 = dMgr.GetPersoon(id4);
            Persoon persoon5 = dMgr.GetPersoon(id5);
            return Json(dMgr.GetComparisonPersonNumberOfTweetsOverTime(persoon1,persoon2,persoon3,persoon4,persoon5), JsonRequestBehavior.AllowGet);
        }
    }
}