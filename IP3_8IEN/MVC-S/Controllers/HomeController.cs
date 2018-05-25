﻿using System.Web.Mvc;
using IP3_8IEN.BL;
using IP3_8IEN.BL.Domain.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IP3_8IEN.BL.Domain.Gebruikers;
using System.IO;
using System.Web;
using IP3_8IEN.BL.Domain.Dashboard;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Text;
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;
using MVC_S.Models;

namespace MVC_S.Controllers
{   /*[RequireHttps]*/
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
            Persoon persoon = dMgr.GetPersoonWithTewerkstelling(automplete);

            string screenname = persoon.Twitter;
            ViewBag.TWITIMAGE = dMgr.GetImageString(screenname);
            ViewBag.TWITBANNER = dMgr.GetBannerString(screenname);

            return View(persoon);
        }
        public ActionResult Personen(int onderwerpId = 231)
        {
            Persoon persoon = dMgr.GetPersoonWithTewerkstelling(onderwerpId);

            ViewBag.TWITIMAGE = dMgr.GetImageString(persoon.Twitter);
            ViewBag.TWITBANNER = dMgr.GetBannerString(persoon.Twitter);

            return View(persoon);
        }

        public ActionResult Themas(int onderwerpId = 500)
        {
            //Thema thema = xMgr.GetThema(onderwerpId);
            /*  verwijder onderstaande region
             *      zodra er via bovenstaande methode
             *      een thema kan binnengehaald worden
             *      en vervang de xMgr met de correcte mgr*/
            #region create default thema
            Thema thema = new Thema()
            {
                OnderwerpId = onderwerpId,
                Naam = "het nieuws",
                ThemaString = "het nieuws",
                Beschrijving = "wat er in het nieuws over wordt gesproken",
                Hashtags = new List<string>()
                {
                    "vtmnieuws",
                    "vrtjournaal"
                },
                SubjectMessages = new List<SubjectMessage>()
                {
                    new SubjectMessage()
                    {
                        SubjectMsgId = 10000
                    }
                }
            };
            #endregion
            #region create searchstring
            StringBuilder searchString = new StringBuilder();
            searchString.Append("https://twitter.com/search?q=");
            for (int i = 0; i < thema.Hashtags.Count; i++)
            {
                if (i > 0)
                {
                    searchString.Append(" OR ");
                }
                searchString.Append($"%23{thema.Hashtags.ElementAt(i)}");
            }
            ViewBag.SearchString = searchString.ToString();
            #endregion
            return View(thema);
        }

        public ActionResult Organisatie(int onderwerpId = 22)
        {
            string screenname = dMgr.GetOrganisatie(onderwerpId).Twitter;
            System.Diagnostics.Debug.WriteLine("Screenname: " + screenname);
            ViewBag.TWITIMAGE = dMgr.GetImageString(screenname);
            ViewBag.TWITBANNER = dMgr.GetBannerString(screenname);
            return View(dMgr.GetOrganisatie(onderwerpId));
        }


        public ActionResult Alerts(int alertId = 1) => View(gMgr.GetAlert(alertId));

        public ActionResult WeeklyReview(int weeklyReviewId = 0)
        {
            WeeklyReview wr = new WeeklyReview()
            {
                WeeklyReviewId = weeklyReviewId
            };
            return View(wr);
        }

        public ActionResult UserDashBoard()
        {
            //Dashbord van ingelogde gebruiker ophalen
            //Hier zit voorlopig enkel update logica 
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
                OnderwerpId = 500,
                Naam = "het nieuws",
                ThemaString = "het nieuws",
                Beschrijving = "wat er in het nieuws over wordt gesproken",
                Hashtags = new List<string>()
                {
                    "vtmnieuws",
                    "vrtjournaal"
                },
                SubjectMessages = new List<SubjectMessage>()
                {
                    new SubjectMessage()
                    {
                        SubjectMsgId = 10000
                    }
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

        public ActionResult InitializeAdmins()
        {
            aMgr.CreateRolesandUsers();
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
            dashMgr.DashbordInitGraphs(dashMgr.AddDefaultDashBord().DashbordId);
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
            string init = "[0,1,2,3,4,5,6,7,8,9]";
            //ViewData["init"] = init;

            List<GraphData> data = dMgr.GetTweetsPerDag(persoon, 20);
            ViewBag.DATA = data;


            ApplicationUser currUser = aMgr.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Dashbord dash;
            if (currUser != null) {
                string userName = currUser.UserName;
                Gebruiker user = gMgr.FindUser(userName);
                dash = dashMgr.GetDashboardWithFollows(user);
            } else
            {
                dash = dashMgr.GetDefaultDashboard();
            }
            ViewBag.INIT = dash.ZonesOrder;
            dashMgr.GetDashItems().Where(d => d.AdminGraph == true);
            ViewBag.AANTALADMIN = dashMgr.GetDashItems().Where(d => d.AdminGraph == true).Count();
            
            //GraphDataViewModel model = new GraphDataViewModel { dash = dash,
            //};
            return View(dash);
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
            return Json(dMgr.GetComparisonPersonNumberOfTweetsOverTime(persoon1, persoon2, persoon3, persoon4, persoon5), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DashItem(int id)
        {
            Persoon persoon = dMgr.GetPersoon(170);
            List<GraphData> data = dMgr.GetTweetsPerDag(persoon, 20);
            ViewBag.DATA = data;
            //IEnumerable<DashItem> dashItem = dashMgr.GetDashItems();
            return View();
        }

        public ActionResult GetJson(List<GraphData> data)
        {
            string bla = null;
            JsonResult d = Json(data, JsonRequestBehavior.AllowGet);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Grafiektest3()
        {
            ApplicationUser currUser = aMgr.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string userName = currUser.UserName;
            Gebruiker user = gMgr.FindUser(userName);
            Dashbord dash = dashMgr.GetDashboardWithFollows(user);
            return View(dash);
        }

        public ActionResult GetJsonFromGraphData(int id)
        {
            //IEnumerable<GraphData> list2 = dashMgr.GetDashItemWithGraph(id).Graphdata;
            List<GraphData> list = dashMgr.ExtractGraphList(id);
            var json = Json(list, JsonRequestBehavior.AllowGet);
            return json;
            return null;
        }

        public ActionResult GetTweets(int persoonId,int aantaldagen )
        {
            Persoon persoon = dMgr.GetPersoon(persoonId);
            //test debug//
            List<GraphData> lijst = dMgr.GetTweetsPerDag(persoon, aantaldagen);
            //////////////
            return Json(dMgr.GetTweetsPerDag(persoon, aantaldagen), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveTilezonesOrder(int dashId,string zonesorder)
        {
            dashMgr.updateTilezonesOrderDashboard(dashId, zonesorder);
            return RedirectToAction("Grafiektest2");
        }
    }
}