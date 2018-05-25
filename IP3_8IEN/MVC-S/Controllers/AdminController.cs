using IP3_8IEN.BL;
using IP3_8IEN.BL.Domain.Data;
using IP3_8IEN.BL.Domain.Gebruikers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using IP3_8IEN.BL.Domain.Dashboard;
using System.Linq;
using MVC_S.Models;

namespace MVC_S.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IDataManager _dataManager;
        private IGebruikerManager _gebrManager;
        private IDashManager _dashManager;
        private ApplicationUserManager _userManager;


        public AdminController()//(ApplicationUserManager userManager, HostingEnvironment environment)
        {
            // Inject the datacontext and userManager Dependencies
            _userManager = new ApplicationUserManager();
            _dataManager = new DataManager();
            _gebrManager = new GebruikerManager();
            _dashManager = new DashManager();
        }


        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult User()
        {
            IEnumerable<ApplicationUser> users = _userManager.GetUsers();
            users = _gebrManager.GetUsersInRoles(users, "User");

            return View(users);
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
            ApplicationUser user = _userManager.FindById(id);
            return View(user);
        }

        // HTTPPOST Controller action to edit user
        [HttpPost]
        public ActionResult EditUser(ApplicationUser model)
        {
            //Get User by the Email passed in.
            //It's better practice to find user by the Id, (without exposing the id to the view).
            var user = _userManager.FindByEmail(model.Email);

            //edit user: replace values of UserViewModel properties 
            user.AchterNaam = model.AchterNaam;
            user.VoorNaam = model.VoorNaam;
            user.UserName = model.UserName;
            user.Geboortedatum = model.Geboortedatum;
            user.PhoneNumber = model.PhoneNumber;

            //add user to the datacontext (database) and save changes
            _userManager.Update(user);

            return RedirectToAction("User");
        }

        [HttpGet]
        public ActionResult DeleteUser(string id)
        {
            ApplicationUser user = _userManager.FindById(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult DeleteUser(string id, FormCollection collection)
        {
            try
            {
                ApplicationUser user = _userManager.FindById(id);
                _userManager.Delete(user);

                // We gaan de gebruiker (gelinkt met objecten in de Db)
                //  niet echt deleten maar overschrijven met anonieme data
                _gebrManager.DeleteUser(id);

                return RedirectToAction("User");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult DetailsUser(string id)
        {
            ApplicationUser user = _userManager.FindById(id);
            return View(user);
        }

        [HttpGet]
        public ActionResult Persoon()
        {
            IEnumerable<Persoon> personen = _dataManager.GetPersonen();
            return View(personen);
        }

        [HttpGet]
        public ActionResult DetailsPersoon(int id = 1)
        {
            Persoon persoon = _dataManager.GetPersoon(id);
            return View(persoon);
        }

        [HttpGet]
        public ActionResult EditPersoon(int id)
        {
            Persoon persoon = _dataManager.GetPersoon(id);
            return View(persoon);
        }

        [HttpPost]
        public ActionResult EditPersoon(int id, Persoon persoon)
        {
            if (ModelState.IsValid)
            {
                _dataManager.ChangePersoon(persoon);

                return RedirectToAction("Persoon");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Organisatie()
        {
            IEnumerable<Organisatie> organisaties = _dataManager.GetOrganisaties();
            return View(organisaties);
        }

        [HttpGet]
        public ActionResult DetailsOrganisatie(int id)
        {
            Organisatie organisatie = _dataManager.GetOrganisatie(id);
            return View(organisatie);
        }

        [HttpGet]
        public ActionResult EditOrganisatie(int id)
        {
            Organisatie organisatie = _dataManager.GetOrganisatie(id);
            return View(organisatie);
        }

        [HttpPost]
        public ActionResult EditOrganisatie(int id, Organisatie organisatie)
        {
            if (ModelState.IsValid)
            {
                _dataManager.ChangeOrganisation(organisatie);

                return RedirectToAction("Organisatie");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Grafiek()
        {
            // enkel grafieken aangemaakt in de AdminController opvragen
            //TODO: implementatie Details, Edit, Delete
            IEnumerable<DashItem> dashItems = _dashManager.GetDashItems().Where(d => d.AdminGraph == true);

            return View(dashItems);
        }

        [HttpGet]
        public ActionResult DetailsGrafiek(int id)
        {
            DashItem dashItem = _dashManager.GetDashItems().FirstOrDefault(d => d.DashItemId == id);

            return View(dashItem);
        }

        [HttpGet]
        public ActionResult DeleteGrafiek(int id)
        {
            DashItem dashItem = _dashManager.GetDashItems().FirstOrDefault(d => d.DashItemId == id);

            return View(dashItem);
        }

        [HttpPost]
        public ActionResult DeleteGrafiek(int id, FormCollection collection)
        {
            try
            {
                _dashManager.RemoveDashItem(id);
                return RedirectToAction("Grafiek");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreateGrafiekInput()
        {
            IEnumerable<Persoon> ObjList = _dataManager.GetPersonen().ToList();
            List<string> names = ObjList.Select(p => p.Naam).ToList();
            ViewData["names"] = names;
            return View();
        }

        [HttpPost]
        public ActionResult CreateGrafiekLine(string automplete)
        {
            string naam = automplete;
            Persoon p = _dataManager.GetPersoon(naam);

            ViewBag.naam = automplete;

            //Zie dat je bent ingelogd
            //TODO: redirect naar inlog pagina <--
            ApplicationUser currUser = _userManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string userName = currUser.UserName;
            Gebruiker user = _gebrManager.FindUser(userName);

            int nDagen = 10; // <-- voorlopig default

            // =============== Opslaan grafiek : opgesplitst om te debuggen =================== //
            List<IP3_8IEN.BL.Domain.Dashboard.GraphData> graphDataList = _dataManager.GetTweetsPerDag(p, nDagen);
            IP3_8IEN.BL.Domain.Dashboard.DashItem newDashItem = _dashManager.CreateDashitem(true, "Line", naam);
            IP3_8IEN.BL.Domain.Dashboard.Follow follow = _dashManager.CreateFollow(newDashItem.DashItemId, p.OnderwerpId);
            IP3_8IEN.BL.Domain.Dashboard.DashItem dashItem = _dashManager.SetupDashItem(user, follow);
            _dashManager.LinkGraphsToUser(graphDataList, dashItem.DashItemId);
            // ================================================================================ //

            Dashbord dash = _dashManager.GetDashboardWithFollows(user);
            return View();
        }

        [HttpGet]
        public ActionResult CreateRankingInput()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRankingInput(string naam, int aantal, int interval)
        {
            RankViewModel rankModel = new RankViewModel()
            {
                Naam = naam,
                Aantal = aantal,
                interval = interval
            };

            TempData["rankModel"] = rankModel;

            return RedirectToAction("CreateRanking");
        }

        [HttpGet]
        public ActionResult CreateRanking()
        {
            RankViewModel rankModel = TempData["rankModel"] as RankViewModel;

            string naam = rankModel.Naam;
            int aantal = rankModel.Aantal;
            int interval = rankModel.interval;

            //Zie dat je bent ingelogd
            //TODO: redirect naar inlog pagina <--
            ApplicationUser currUser = _userManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string userName = currUser.UserName;
            Gebruiker user = _gebrManager.FindUser(userName);

            // =============== Opslaan grafiek : opgesplitst om te debuggen =================== //
            List<GraphData> graphDataList = _dataManager.GetRanking(aantal,interval,true);
            DashItem newDashItem = _dashManager.CreateDashitem(true, "Rank", naam);
            List<int> arrayPersoonId = _dataManager.ExtractListPersoonId(graphDataList);
            List<Follow> follows = _dashManager.CreateFollow(newDashItem.DashItemId, arrayPersoonId);
            DashItem dashItem = _dashManager.SetupDashItem(user, follows);
            _dashManager.LinkGraphsToUser(graphDataList, dashItem.DashItemId);
            // ================================================================================ //

            Dashbord dash = _dashManager.GetDashboardWithFollows(user);
            return View();
        }

        [HttpGet]
        public ActionResult CreateGrafiekDonut()
        {
            return View();
        }
    }
}