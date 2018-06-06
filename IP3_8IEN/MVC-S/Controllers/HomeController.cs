using System.Web.Mvc;
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
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;
using MVC_S.Models;
using System.Text;
using Newtonsoft.Json;
using System;
using System.Web.Hosting;
using System.Web.Security;
using System.Collections.ObjectModel;
using IP_8IEN.BL.Domain.Dashboard;

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
            

            ////Probably not best practice to periodically execute methods but it works
            //HostingEnvironment.QueueBackgroundWorkItem(ct => WeeklyReview(gMgr));
            //HostingEnvironment.QueueBackgroundWorkItem(ct => RetrieveAPIData(dMgr));
        }

        private async Task RetrieveAPIData(IDataManager dMgr)
        {
            while (true)
            {
                //wait 3h and get new data from textgain
                Thread.Sleep(10800000);
                await Task.Run(() =>
                {
                    dMgr.ApiRequestToJson(true);
                });
            }
        }

        private async Task WeeklyReview(IGebruikerManager gMgr)
        {
            while (true)
            {
                //wait 1w and send out weekly review
                Thread.Sleep(604800000);
                await Task.Run(() =>
                {
                    gMgr.WeeklyReview();
                });
            }
        }

        public ActionResult Index()
        {
            ViewDataValue vdv = dMgr.GetViewDataValue("HomePage");

            if (vdv != null)
            {
                ViewBag.Welcome = vdv.StringValue;
            } else
            {
                ViewBag.Welcome = @"8IEN. helpt u bij het up-to-date blijven over alles wat er gaande is binnen het Vlaamse politieke milieu.
                Op het dashboard kan je een reeks reeds ingestelde grafieken bekijken, maar ook persoonlijke grafieken aanmaken";
            }
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

        [HttpPost]
        public ActionResult Contact(string naam, string email, string bericht)
        {
            gMgr.SendMail(naam, email, bericht, "Contact formulier");
            return View();
        }

        public ActionResult Dashboard()
        {
            //try
            //{
            //    ApplicationUser appUser = aMgr.FindById(User.Identity.GetUserId());
            //    string userName = appUser.UserName;
            //    Gebruiker user = gMgr.FindUser(userName);

            //    Dashbord dashbord = dashMgr.GetDashboardWithFollows(user);
            //    dashbord = dashMgr.UpdateDashboard(dashbord); // <-- zien dat elk DashItem up-to-date is
            //}
            //catch
            //{
            //}

            bool ingelogd = false;

            IEnumerable<Persoon> ObjList = dMgr.GetPersonen().ToList();
            List<string> names = ObjList.Select(p => p.Naam).ToList();
            ViewData["names"] = names;

            Persoon persoon = dMgr.GetPersoon(170);
            int aantalTweets = dMgr.GetNumber(persoon);
            //int aantalTweets = 69;
            ViewBag.NUMMER1 = aantalTweets;
            ViewBag.naam1 = persoon.Naam;
            //System.Diagnostics.Debug.WriteLine("tweets per dag"+aantalTweets);
            int[] init = { 0, 1, 3, 2, 8, 6, 5, 4, 9, 7 };
            //ViewData["init"] = init;
            List<double> spark = dMgr.GetTotalMessagesSparkline();
            spark.Reverse();
            ViewBag.msgsSpark = spark;
            ViewBag.percent = dMgr.GetstijgingTweets();

            //List<GraphData> data = dMgr.GetTweetsPerDagList(persoon, 20);
            //ViewBag.DATA = data;


            ApplicationUser currUser = aMgr.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            Dashbord dash;
            if (currUser != null)
            {
                string userName = currUser.UserName;
                Gebruiker user = gMgr.FindUser(userName);
                dash = dashMgr.GetDashboardWithFollows(user);
                ingelogd = true;
            }
            else
            {
                //not jet ready
                //have to add defaultdash
                string userName = "default@gmail.be";
                Gebruiker user = gMgr.FindUser(userName);
                dash = dashMgr.GetDashboardWithFollows(user);
            }

            ViewBag.Ingelogd = ingelogd;
            ViewBag.INIT = dash.ZonesOrder;
            //dashMgr.GetDashItems().Where(d => d.AdminGraph == true);
            ViewBag.AANTAL = dashMgr.GetDashItems().Where(d => d.AdminGraph == true).Count();
            //GraphDataViewModel model = new GraphDataViewModel { dash = dash,
            //};
            return View(dash);
        }

        //Get:
        [HttpPost]
        public ActionResult Personen(string automplete)
        {
            Persoon persoon = dMgr.GetPersoonWithTewerkstelling(automplete);

            string screenname = persoon.Twitter;
            ViewBag.TWITIMAGE = dMgr.GetImageString(screenname);
            ViewBag.TWITBANNER = dMgr.GetBannerString(screenname);

            return View(persoon);
        }



        public ActionResult Personen(int onderwerpId)
        {
            Persoon persoon = dMgr.GetPersoonWithTewerkstelling(onderwerpId);

            ViewBag.TWITIMAGE = dMgr.GetImageString(persoon.Twitter);
            ViewBag.TWITBANNER = dMgr.GetBannerString(persoon.Twitter);
            ViewBag.Vermeldingen = dMgr.GetMentionCountByName(persoon.Twitter);
            ViewBag.VaakVoorkomendeWoorden = dMgr.TopWordsCountByPerson(persoon);
            ViewBag.VaakVoorkomendeVerhalen = dMgr.TopStoryCountByPerson(persoon);
            ViewBag.VaakVoorkomendeTermen = dMgr.TopHashtagCountByPerson(persoon);

            return View(persoon);
        }

        [HttpGet]
            public ActionResult Themas(int id)
        {
            Thema theme = dMgr.GetThemas().FirstOrDefault(t => t.OnderwerpId == id);

            theme.Hashtags = new Collection<string>();

            theme.Hashtags.Add(theme.Hashtag1);
            theme.Hashtags.Add(theme.Hashtag2);
            theme.Hashtags.Add(theme.Hashtag3);
            theme.Hashtags.Add(theme.Hashtag4);

            StringBuilder searchString = new StringBuilder();
            searchString.Append("https://twitter.com/search?q=");
            for (int i = 0; i < theme.Hashtags.Count; i++)
            {
                if (i > 0)
                {
                    searchString.Append(" OR ");
                }
                searchString.Append($"%23{theme.Hashtags.ElementAt(i)}");
            }
            ViewBag.SearchString = searchString.ToString();

            return View(theme);
        }

        public ActionResult Organisatie(int onderwerpId = 22)
        {
            string screenname = dMgr.GetOrganisatie(onderwerpId).Twitter;
            System.Diagnostics.Debug.WriteLine("Screenname: " + screenname);
            ViewBag.TWITIMAGE = dMgr.GetImageString(screenname);
            ViewBag.TWITBANNER = dMgr.GetBannerString(screenname);

            ViewBag.Vermeldingen = dMgr.GetMentionCountByName(screenname);
            ViewBag.VaakVoorkomendeWoorden = dMgr.TopWordsCountByOrganisatie(dMgr.GetOrganisatie(onderwerpId));
            ViewBag.VaakVoorkomendeVerhalen = dMgr.TopStoryCountByOrganisatie(dMgr.GetOrganisatie(onderwerpId));
            ViewBag.VaakVoorkomendeTermen = dMgr.TopHashtagCountByOrganisation(dMgr.GetOrganisatie(onderwerpId));


            return View(dMgr.GetOrganisatie(onderwerpId));
        }


        public ActionResult Alerts(int alertId = 1)
        {
            ApplicationUser currUser = aMgr.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string username = currUser.UserName;
            Gebruiker user = gMgr.FindUser(username);


            IEnumerable<Alert> alerts = gMgr.GetAlertsByUser(user);
            alerts = alerts.ToList();
            return View(alerts);
        }

        public ActionResult WeeklyReview(int weeklyReviewId = 0)
        {
            ApplicationUser currUser = aMgr.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string username = currUser.UserName;
            Gebruiker user = gMgr.FindUser(username);

            return View("~/Views/Home/WeeklyReview.cshtml" /* view name*/,
            null /* master name */,
            gMgr.WeeklyReview(user) /* model */);
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
                dashbord = dashMgr.UpdateDashboard(dashbord); // <-- zien dat elk DashItem up-to-date is

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
            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("NotAllowed", "Error");
            }
        }

        public ActionResult Superadmin()
        {
            // note : deze 'if else' kun je gebruiken voor authorisatie
            if (User.IsInRole("SuperAdmin"))
            {
                return RedirectToAction("Index", "Superadmin");
            }
            else
            {
                return RedirectToAction("NotAllowed", "Error");
            }
        }

        public ActionResult Instellingen() => View();

        public ActionResult LijstPersonen() => View(dMgr.GetPersonen());

        public ActionResult LijstThemas()
        {
            List<Thema> themes = dMgr.GetThemas().ToList();

            foreach(Thema theme in themes)
            {
                theme.Hashtags = new Collection<string>();

                theme.AantalVermeldingen = dMgr.GetAantalVermeldingen(theme);

                theme.Hashtags.Add(theme.Hashtag1);
                theme.Hashtags.Add(theme.Hashtag2);
                theme.Hashtags.Add(theme.Hashtag3);
                theme.Hashtags.Add(theme.Hashtag4);
            }

            return View(themes);
        }

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
            try
            {
                //Aanmaken Roles en initialisatie 'SuperAdmin'
                aMgr.CreateRolesandUsers();
                aMgr.AddApplicationGebruikers(Path.Combine(HttpRuntime.AppDomainAppPath, "AddApplicationGebruikers.Json"));
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Initialize()
        {
            // Initializatie systeem //
            // >>>>>>>>> InitializeAdmins() hierboven eerst uitvoeren <<<<<<<<< //

            #region initialisatie blok databank
            dMgr.AddPersonen(Path.Combine(HttpRuntime.AppDomainAppPath, "politici.Json"));
            dMgr.ApiRequestToJson();
            gMgr.AddAlertInstelling(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlertInstelling.json"));
            gMgr.AddAlerts(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlerts.json"));
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
            IEnumerable<Persoon> ObjList = dMgr.GetPersonen().ToList();
            List<string> names = ObjList.Select(p => p.Naam).ToList();
            ViewData["names"] = names;

            Persoon persoon = dMgr.GetPersoon(170);
            int aantalTweets = dMgr.GetNumber(persoon);
            //int aantalTweets = 69;
            ViewBag.NUMMER1 = aantalTweets;
            ViewBag.naam1 = persoon.Naam;
            //System.Diagnostics.Debug.WriteLine("tweets per dag"+aantalTweets);
            int[] init = { 0, 1, 3, 2, 8, 6, 5, 4, 9, 7 };
            //ViewData["init"] = init;


            List<GraphData> data = dMgr.GetTweetsPerDag(persoon, 20);
            ViewBag.DATA = data;
           
            
            ApplicationUser currUser = aMgr.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            
            Dashbord dash;
                if (currUser != null){
                string userName = currUser.UserName;
                Gebruiker user = gMgr.FindUser(userName);
                dash = dashMgr.GetDashboardWithFollows(user);
            }
            else
            {
                //not jet ready
                //have to add defaultdash
                string userName = "default@gmail.be";
                Gebruiker user = gMgr.FindUser(userName);
                dash = dashMgr.GetDashboardWithFollows(user);
            }
            
            
            ViewBag.INIT = dash.ZonesOrder;
            dashMgr.GetDashItems().Where(d => d.AdminGraph == true);
            ViewBag.AANTAL = dashMgr.GetDashItems().Where(d => d.AdminGraph == true).Count();
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

        public ActionResult UserDashBoardById(string name)
        {
            //Dashbord van ingelogde gebruiker ophalen
            try
            {
                //ApplicationUser appUser = aMgr.FindById(User.Identity.GetUserId());
                //string userName = appUser.UserName;
                Gebruiker user = gMgr.FindUser(name);

                Dashbord dashbord = dashMgr.GetDashboardWithFollows(user);
                dashbord = dashMgr.UpdateDashboard(dashbord); // <-- zien dat elk DashItem minstens 3h up-to-date is

                var list = JsonConvert.SerializeObject(dashbord,Formatting.None,new JsonSerializerSettings()
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

        //public ActionResult DashItem(int id)
        //{
        //    // Check user provided credentials with database and if matches write this
        //    FormsAuthentication.SetAuthCookie(model.Id, false);
        //    return View();
        //}

        public ActionResult GetJson(List<GraphData> data)
        {
            string bla = null;
            JsonResult d = Json(data, JsonRequestBehavior.AllowGet);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetJsonFromGraphData(int id)
        {
            //IEnumerable<GraphData> list2 = dashMgr.GetDashItemWithGraph(id).Graphdata;
            List<DataChart> list = dashMgr.ExtractGraphList(id);
            var json = Json(list, JsonRequestBehavior.AllowGet);
            return json;
        }
        public ActionResult GetJsonFromGraphData2(int id)
        {
            //IEnumerable<GraphData> list2 = dashMgr.GetDashItemWithGraph(id).Graphdata;
            List<DataChart2> list = dashMgr.ExtractGraphList2(id);
            var json = Json(list, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult GetTweets(int persoonId, int aantaldagen)
        {
            Persoon persoon = dMgr.GetPersoon(persoonId);
            //test debug//
            List<DataChart> lijst = dMgr.GetTweetsPerDagDataChart(persoon, aantaldagen);
            //////////////
            return Json(dMgr.GetTweetsPerDagDataChart(persoon, aantaldagen), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveTilezonesOrder(int dashId, string zonesorder)
        {
            dashMgr.updateTilezonesOrder(dashId, zonesorder);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public ActionResult CreateChartAantalTweetsPerDag(string politicus,string type,int aantalDagenTerug)
        {
            ApplicationUser currUser = aMgr.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string userName = currUser.UserName;
            Gebruiker user = gMgr.FindUser(userName);

            string naam = politicus;
            Persoon p = dMgr.GetPersoon(naam);

            List<GraphData> graphDataList = dMgr.GetTweetsPerDag(p, aantalDagenTerug);
            DashItem newDashItem = dashMgr.CreateDashitem(false, type, aantalDagenTerug, naam);
            Follow follow = dashMgr.CreateFollow(newDashItem.DashItemId, p.OnderwerpId);
            DashItem dashItem = dashMgr.SetupDashItem(user, follow);
            dashMgr.LinkGraphsToUser(graphDataList, dashItem.DashItemId);
            

            return RedirectToAction("Dashboard");

            
        }
        //[HttpGet]
        //public ActionResult DeleteGrafiek(int id)
        //{
        //    DashItem dashItem = dashMgr.GetDashItems().FirstOrDefault(d => d.DashItemId == id);

        //    return View(dashItem);
        //}

        [HttpGet]
        public ActionResult DeleteGrafiek(int id)
        {
            try
            {
                ApplicationUser currUser = aMgr.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                Dashbord dash;
                if (currUser != null)
                {
                    string userName = currUser.UserName;
                    Gebruiker user = gMgr.FindUser(userName);
                    dash = dashMgr.GetDashboardWithFollows(user);
                    dashMgr.DeleteOneZonesOrder(dash);
                    dashMgr.RemoveDashItem(id);
                }
                else
                {
                    //not jet ready
                    //have to add defaultdash
                    //default redirect to inlog or alert to log in
                }
                
                return RedirectToAction("Dashboard");
            }
            catch
            {
                return RedirectToAction("Dashboard");
            }
        }

        public ActionResult GetTopStory(int id , int aantal)
        {
            Persoon persoon = dMgr.GetPersoon(id);
            List<GraphData> woorden = dMgr.GetTopStoryByPolitician(persoon);
            return Json(dMgr.GetTweetsPerDag(persoon, 20), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTopMentions(int id)
        {
            Persoon persoon = dMgr.GetPersoon(id);
            return Json(dMgr.GetTweetsPerDag(persoon, 20), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFrequenteWoorden(string id, int aantal)
        {
            Persoon pers = dMgr.GetPersoon(id);
            Persoon persoon = dMgr.GetPersoonWithSjctMsg(pers.OnderwerpId);
            IEnumerable<SubjectMessage> subjectMsgs = persoon.SubjectMessages.ToList();
            List<GraphData> woorden = dMgr.FrequenteWoorden(persoon.SubjectMessages, aantal);

            return Json(woorden, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllPersonsList()
        {
            List<Persoon> personen = dMgr.GetPersonenOnly().ToList();
            return Json(personen, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetPersoon(string id)
        //{
        //    Persoon persoon = dMgr.GetPersoon(id);
        //    return Json(persoon, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetAlertsDropDown()
        {
            return Content("Some data"); // Of whatever you need to return.
        }

        public ActionResult Notification()
        {
            List<Alert> alerts = gMgr.GetAlerts().ToList().OrderByDescending(a => a.CreatedOn).Take(5).ToList();
            return PartialView(alerts);
        }

        public ActionResult Privacy()
        {
            return View();
        }


        public PartialViewResult _partialOne(ViewDataModel one)
        {
            ViewDataValue vdvP = dMgr.GetViewDataValue("Personen");
            ViewDataValue vdvO = dMgr.GetViewDataValue("Organisaties");
            ViewDataValue vdvT = dMgr.GetViewDataValue("Themas");

            if(vdvP != null)
            {
                one.Personen = vdvP.StringValue;

                //ViewBag.Personen = vdvP.StringValue;
                ViewBag.Organisaties = vdvO.StringValue;
                ViewBag.Themas = vdvT.StringValue;
            } else
            {
                ViewBag.Personen = "Personen";
                ViewBag.Organisaties = "Organisaties";
                ViewBag.Themas = "Themas";
            }

            return PartialView("~/Views/Home/_partialOne.cshtml", one);
            //return PartialView();
        }

        [HttpGet]
        public ActionResult GetP()
        {
            ViewDataValue vdvP = dMgr.GetViewDataValue("Personen");
            if (vdvP != null)
            {
                string vdv = vdvP.StringValue;
                return Json(vdv, JsonRequestBehavior.AllowGet);
            } else
            {
                return Json("Personen", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetO()
        {
            ViewDataValue vdvP = dMgr.GetViewDataValue("Organisaties");
            if (vdvP != null)
            {
                string vdv = vdvP.StringValue;
                return Json(vdv, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Organisaties", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetT()
        {
            ViewDataValue vdvP = dMgr.GetViewDataValue("Themas");
            if (vdvP != null)
            {
                string vdv = vdvP.StringValue;
                return Json(vdv, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Themas", JsonRequestBehavior.AllowGet);
            }
        }
    }
}