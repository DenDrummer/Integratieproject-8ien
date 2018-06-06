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
                ViewDataValue vdv = _dataManager.GetViewDataValue("HomePage");
            if (vdv != null)
            {
                return View(vdv);
            } else
            {
                vdv = new ViewDataValue()
                {
                    Name = "HomePage",
                    StringValue = @"8IEN. helpt u bij het up-to-date blijven over alles wat er gaande is binnen het Vlaamse politieke milieu.
                Op het dashboard kan je een reeks reeds ingestelde grafieken bekijken, maar ook persoonlijke grafieken aanmaken"
                };

                return View(vdv);
            }
        }

        [HttpPost]
        public ActionResult HomePage(ViewDataValue vdv)
        {
            ViewDataValue vdvDb = _dataManager.GetViewDataValue("HomePage");
            if (vdvDb != null)
            {
                vdvDb.Name = vdv.Name;
                vdvDb.StringValue = vdv.StringValue;
                _dataManager.UpdateViewDataValue(vdvDb);
            } else
            {
                _dataManager.AddViewDataValue(vdv);
            }
            
            return RedirectToAction("Globals", "SuperAdmin");
        }

        [HttpGet]
        public ActionResult Benamingen()
        {
            ViewDataModel vdm = new ViewDataModel();
            //List<ViewDataValue> VdvList = new List<ViewDataValue>();

            ViewDataValue vdvP = _dataManager.GetViewDataValue("Personen");
            if (vdvP != null)
            {
                vdm.Personen = vdvP.Name;
            }
            else
            {
                vdvP = new ViewDataValue()
                {
                    Name = "Personen",
                    StringValue = @"Personen"
                };
                _dataManager.AddViewDataValue(vdvP);
                vdm.Personen = vdvP.Name;
            }

            ViewDataValue vdvO = _dataManager.GetViewDataValue("Organisaties");
            if (vdvO != null)
            {
                vdm.Organisaties = vdvO.Name;
            }
            else
            {
                vdvO = new ViewDataValue()
                {
                    Name = "Organisaties",
                    StringValue = @"Organisaties"
                };
                _dataManager.AddViewDataValue(vdvO);
                vdm.Organisaties = vdvO.Name;
            }

            ViewDataValue vdvT = _dataManager.GetViewDataValue("Themas");
            if (vdvT != null)
            {
                vdm.Themas = vdvT.Name;
            }
            else
            {
                vdvT = new ViewDataValue()
                {
                    Name = "Themas",
                    StringValue = @"Themas"
                };
                _dataManager.AddViewDataValue(vdvT);
                vdm.Themas = vdvT.Name;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Benamingen(ViewDataModel vdm)
        {
            ViewDataValue vdvP = _dataManager.GetViewDataValue("Personen");
            ViewDataValue vdvO = _dataManager.GetViewDataValue("Organisaties");
            ViewDataValue vdvT = _dataManager.GetViewDataValue("Themas");

            vdvP.StringValue = vdm.Personen;
            _dataManager.UpdateViewDataValue(vdvP);

            vdvO.StringValue = vdm.Organisaties;
            _dataManager.UpdateViewDataValue(vdvO);

            vdvT.StringValue = vdm.Themas;
            _dataManager.UpdateViewDataValue(vdvT);

            //ViewDataValue vdv;
            //foreach(ViewDataValue vdval in vdm.VdvList)
            //{
            //    vdv = _dataManager.GetViewDataValue(vdval.Name);
            //    if (vdv != null)
            //    {
            //        vdv.Name = vdval.Name;
            //        vdv.StringValue = vdval.StringValue;
            //        _dataManager.UpdateViewDataValue(vdv);
            //    }
            //    else
            //    {
            //        _dataManager.AddViewDataValue(vdv);
            //    }
            //}
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