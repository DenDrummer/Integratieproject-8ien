using IP3_8IEN.BL;
using IP3_8IEN.BL.Domain.Gebruikers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_S.Controllers
{
    public class SuperadminController : Controller
    {
        private IDataManager _dataManager;
        private IGebruikerManager _gebrManager;
        private IDashManager _dashManager;
        private ApplicationUserManager _userManager;


        public SuperadminController()//(ApplicationUserManager userManager, HostingEnvironment environment)
        {
            // Inject the datacontext and userManager Dependencies
            _userManager = new ApplicationUserManager();
            _dataManager = new DataManager();
            _gebrManager = new GebruikerManager();
            _dashManager = new DashManager();
        }

        // GET: Superadmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Adminlijst()
        {
            IEnumerable<ApplicationUser> users = _userManager.GetUsers();

            IEnumerable<Gebruiker> gebruikers = _gebrManager.GetUsers().Where(g => g.Role == "Admin");
            users = _gebrManager.GetUsersInRoles(users);

            return View(users);
        }

        public ActionResult SMBeheren()
        {
            return View();
        }
    } 

}