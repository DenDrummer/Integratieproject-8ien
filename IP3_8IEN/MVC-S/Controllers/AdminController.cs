using IP_8IEN.BL;
using IP_8IEN.BL.Domain.Gebruikers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MVC_S.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;


        public AdminController()//(ApplicationUserManager userManager, HostingEnvironment environment)
        {
            // Inject the datacontext and userManager Dependencies
            _userManager = new ApplicationUserManager();
        }


        // GET: Admin
        public ActionResult Index()
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
    }
}