using IP3_8IEN.BL;
using System.Web.Mvc;

namespace UI_MVC3.Controllers
{
    public class HomeController : Controller
    {
        private IDataManager mgr = new DataManager();

        public ActionResult Index()
        {
            mgr.AddMessages(Server.MapPath("~\\textgaindump.json"));
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