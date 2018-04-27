using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices.Internal;
using System.Web.Mvc;
using IP_8IEN.BL;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using IP_8IEN.BL.Domain.Gebruikers;
using Microsoft.Ajax.Utilities;

namespace MVC_S.Controllers
{
    public class HomeController : Controller
    {
        private IDataManager dMgr;
        private IGebruikerManager gMgr;

        public HomeController()
        {
            // Hier wordt voorlopig wat testdata doorgegeven aan de 'Managers'
            dMgr = new DataManager();
            gMgr = new GebruikerManager();

            //-- Laat deze twee in commentaar staan --//
            //dMgr.ApiRequestToJson();
            //dMgr.AddMessages(@"C:\Users\Nathan\Desktop\api.json");
            //--                                    --//

            dMgr.AddMessages(Path.Combine(HttpRuntime.AppDomainAppPath, "textgaintest2.json"));

            dMgr.AddOrganisation("Groen");
            dMgr.AddOrganisation("Groen");
            dMgr.AddOrganisation("VLD");
            dMgr.AddTewerkstelling("Pascal Smet", "Groen");
            dMgr.AddTewerkstelling("Tom Van Grieken", "Groen");

            gMgr.AddGebruikers(Path.Combine(HttpRuntime.AppDomainAppPath, "AddGebruikersInit.Json"));
            gMgr.AddAlertInstelling(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlertInstelling.json"));
            gMgr.AddAlerts(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlerts.json"));
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


        public ActionResult grafiektest()
        {
            //List<string> list = new List<string>();
            //list.Add("Ivo");
            //list.Add("Sam");
            //list.Add("vic");
            //list.Add("Steffi");
            //list.Add("Tommie");
            //list.Add("Jordy");
            var list = dMgr.ReadOnderwerpen();
            List<string> bla = new List<string>();
            var beschrijvingen = list.Select(x => x.Beschrijving).Take(20);
            //foreach (var t in beschrijvingen)
            //{
            //   System.Diagnostics.Debug.WriteLine("de volgende fucker" + t); 
            //}
            List<Gebruiker> users = new List<Gebruiker>(gMgr.GetGebruikers());
            StringBuilder sb1 = new StringBuilder(); 
            foreach (var u in users)
            {
                //System.Diagnostics.Debug.WriteLine("de volgende fucker" + u.Naam);
                sb1.Append(u.Naam + ";");
            }
            string lijstnamen = sb1.ToString();
            System.Diagnostics.Debug.WriteLine(sb1);
            for (int i = 0; i < 20; i++)
            {
                bla.Add(i+";");
            }

            string blabla = "1;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20";
            ViewBag.Message = "Your grafiek page.";
            ViewBag.BESCH = lijstnamen;

            return View();
        }
        public ActionResult onechart()
        {
           

            return View();
        }

       

    }
}