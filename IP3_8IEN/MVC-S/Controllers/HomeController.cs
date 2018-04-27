using System.Web.Mvc;
using IP_8IEN.BL;
using System.IO;
using System.Web;
using System.Threading;
using System.Web.Hosting;
using System.Threading.Tasks;

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

            //dMgr.AddPersonen(Path.Combine(HttpRuntime.AppDomainAppPath, "politici.Json"));
            //dMgr.ApiRequestToJson();
            //dMgr.CountSubjMsgsPersoon();
            dMgr.ReadOnderwerpenWithSubjMsgs();


            //gMgr.AddGebruikers(Path.Combine(HttpRuntime.AppDomainAppPath, "AddGebruikersInit.Json"));
            //gMgr.AddAlertInstelling(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlertInstelling.json"));
            //gMgr.AddAlerts(Path.Combine(HttpRuntime.AppDomainAppPath, "AddAlerts.json"));

            //dMgr.GetAlerts();

            HostingEnvironment.QueueBackgroundWorkItem(ct => SendMailAsync(dMgr));

        }
        private async Task SendMailAsync(IDataManager dMgr)
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    dMgr.GetAlerts();
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
    }
}