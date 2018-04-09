using IP3_8IEN.BL;
using System.Web.Mvc;

namespace UI_MVC3.Controllers
{
    public class HomeController : Controller
    {
        private IDataManager dMgr;
        private IGebruikerManager gMgr;

        public HomeController()
        {
            // Hier wordt voorlopig gewoon wat testdata doorgegeven aan de 'Managers'
            dMgr = new DataManager();
            gMgr = new GebruikerManager();
            
            dMgr.AddMessages($"c:\\textgaindump.json");
            gMgr.AddGebruikers($"c:\\AddGebruikersInit.Json");
            gMgr.AddAlertInstelling($"c:\\AddAlertInstelling.json");
            gMgr.AddAlerts($"c:\\AddAlerts.json");
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