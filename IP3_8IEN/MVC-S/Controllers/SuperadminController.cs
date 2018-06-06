using IP3_8IEN.BL;
using IP3_8IEN.BL.Domain.Data;
using IP3_8IEN.BL.Domain.Gebruikers;
using Microsoft.AspNet.Identity;
using MVC_S.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            users = _gebrManager.GetUsersInRoles(users, "Admin");

            return View(users);
        }

        [HttpGet]
        public ActionResult EditAdmin(string id)
        {
            ApplicationUser user = _userManager.FindById(id);
            return View(user);
        }
        // HTTPPOST Controller action to edit user
        [HttpPost]
        public ActionResult EditAdmin(ApplicationUser model)
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

            return RedirectToAction("AdminLijst");
        }

        [HttpGet]
        public ActionResult DetailsAdmin(string id)
        {
            ApplicationUser user = _userManager.FindById(id);
            return View(user);
        }

        [HttpGet]
        public ActionResult DeleteAdmin(string id)
        {
            ApplicationUser user = _userManager.FindById(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult DeleteAdmin(string id, FormCollection collection)
        {
            try
            {
                ApplicationUser user = _userManager.FindById(id);
                _userManager.Delete(user);

                // We gaan de gebruiker (gelinkt met objecten in de Db)
                //  niet echt deleten maar overschrijven met anonieme data
                _gebrManager.DeleteUser(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreateAdmin()
        {
            //kan ook gewone users aanmaken
            UserVIewModel model = new UserVIewModel();
            model.Roles = new List<SelectListItem>();
            model.Roles.Add(new SelectListItem() { Text = "Admin", Value = "Admin" });
            model.Roles.Add(new SelectListItem() { Text = "User", Value = "User" });

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateAdmin(UserVIewModel user)
        {
            try
            {
                //Als username geven we default het emailadres mee
                _userManager.AddApplicationGebruiker(user.Email, user.Voornaam, user.Naam,
                    user.Email, DateTime.Now, user.Passw, user.Role);

                return RedirectToAction("AdminLijst");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Globals()
        {
            return View();
        }

        [HttpGet]
        public ActionResult HomePage()
        {
            try
            {
                ViewDataValue vdv = _dataManager.GetViewDataValue("HomePage");
            } catch
            {
                ViewDataValue vdv = new ViewDataValue()
                {
                    Name = "HomePage",
                    StringValue = ""
                };
            }
            
            return View();
        }

        [HttpGet]
        public ActionResult Faq()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Privacy()
        {

            return View();
        }

        [HttpGet]
        public ActionResult InitializeDb()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InsertOnderwerpen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertOnderwerpen(string file)
        {
            // voorbeeld : politici.json
            // initialiseert automatisch personen, organisaties en tewerkstellingen
            _dataManager.AddPersonen(Path.Combine(HttpRuntime.AppDomainAppPath, file));
            return View();
        }

        public ActionResult SMBeheren()
        {
            return View();
        }
    } 

}