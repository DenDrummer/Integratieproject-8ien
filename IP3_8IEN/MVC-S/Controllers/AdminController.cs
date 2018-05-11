﻿using IP_8IEN.BL;
using IP_8IEN.BL.Domain.Data;
using IP_8IEN.BL.Domain.Gebruikers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

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
            IEnumerable<ApplicationUser> users = _userManager.GetUsers();
            return View(users);
        }
        
        public ActionResult User()
        {
            IEnumerable<ApplicationUser> users = _userManager.GetUsers();
            return View(users);
        }

        // HTTPGET Controller action to edit user
        // zoeken op email /username kan conflicten opleveren
        // niet meteen 'good practice' op deze manier
        [HttpGet]
        public async Task<ActionResult> EditUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            return await Task.Run(() => View(user));
        }

        // HTTPPOST Controller action to edit user
        [HttpPost]
        public async Task<ActionResult> EditUser(ApplicationUser model)
        {
            //Get User by the Email passed in.
            //It's better practice to find user by the Id, (without exposing the id to the view).
            var user = await _userManager.FindByEmailAsync(model.Email);

            //edit user: replace values of UserViewModel properties 
            user.AchterNaam = model.AchterNaam;
            user.VoorNaam = model.VoorNaam;
            user.UserName = model.UserName;
            user.Geboortedatum = model.Geboortedatum;
            user.PhoneNumber = model.PhoneNumber;

            //add user to the datacontext (database) and save changes
            await _userManager.UpdateAsync(user);

            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<ActionResult> DeleteUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            return await Task.Run(() => View(user));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id, FormCollection collection)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);

                return await Task.Run(() => RedirectToAction("Index"));
            }
            catch
            {
                return await Task.Run(() => View());
            }
        }

        [HttpGet]
        public async Task<ActionResult> DetailsUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            return await Task.Run(() => View(user));
        }

        [HttpGet]
        public ActionResult Persoon()
        {
            IEnumerable<Persoon> personen = _dataManager.GetPersonen();
            return View(personen);
        }

        [HttpGet]
        public ActionResult DetailsPersoon(int id)
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

                return RedirectToAction("Index");
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

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateGrafiekLine()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateGrafiekLine(Persoon persoon)
        {
            ApplicationUser currUser = await _userManager.FindByIdAsync(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string userName = currUser.UserName;
            Gebruiker user = _gebrManager.FindUser(userName);

            // Als de zoekmthode klaar is wordt het onderwerp door de view meegegeven //
            int id = 231; // <-- Verhofstadt
            int nDagen = 10;
            Persoon p = _dataManager.GetPersoon(id);

            // =============== Opslaan grafiek : opgesplitst om te debuggen =================== //
            List<IP3_8IEN.BL.Domain.Dashboard.GraphData> graphDataList = _dataManager.GetTweetsPerDag(p, user, nDagen);
            IP_8IEN.BL.Domain.Dashboard.DashItem newDashItem = _dashManager.CreateDashitem();
            IP_8IEN.BL.Domain.Dashboard.Follow follow = _dashManager.CreateFollow(newDashItem.DashItemId,p.OnderwerpId);
            IP_8IEN.BL.Domain.Dashboard.DashItem dashItem = _dashManager.SetupDashItem(/*newDashItem, */user, follow);
            _dashManager.LinkGraphsToUser(graphDataList, dashItem.DashItemId);
            // ================================================================================ //

            return View();
        }

        [HttpGet]
        public ActionResult CreateGrafiekDonut()
        {
            return View();
        }
    }
}