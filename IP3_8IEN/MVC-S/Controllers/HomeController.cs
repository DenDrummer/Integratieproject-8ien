using System.Web.Mvc;
using IP_8IEN.BL;
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
            dMgr = new DataManager();
            gMgr = new GebruikerManager();
            
            //dMgr.ApiRequestToJson();

            //dMgr.AddMessages(Server.MapPath("~\\textgaindump.json"));

            dMgr.AddMessages(Path.Combine(HttpRuntime.AppDomainAppPath, "textgaindump.json"));

            dMgr.AddOrganisation("Groen");
            dMgr.AddOrganisation("Groen");
            dMgr.AddOrganisation("VLD");
            dMgr.AddTewerkstelling("Imade", "Annouri", "Groen");
            dMgr.AddTewerkstelling("Annick", "De Ridder", "Groen");

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
    }
}