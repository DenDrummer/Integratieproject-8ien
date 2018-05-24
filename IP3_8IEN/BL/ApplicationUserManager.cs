using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using IP3_8IEN.DAL;
using IP3_8IEN.BL.Domain.Gebruikers;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using Newtonsoft.Json;

namespace IP3_8IEN.BL
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        //IdentityRepository repo =  new IdentityRepository();
        private IGebruikerManager _gebruikerMgr;

        public ApplicationUserManager()
            : base(new IdentityRepository())
        {
            // Configure validation logic for usernames
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Role bij een gebruiker toevoegen
            //CreateRolesandUsers(); // CreateRolesandUsers();
        }

        //// Roles in ASP.Identity
        //private void CreateRolesandUsers()
        //{
        //    // Context moet opgehaald worden uit de repository!
        //    // ApplicationDbContext repo = (ApplicationDbContext)(((IdentityRepository)Store).Context);
        //    IdentityRepository repo = new IdentityRepository();

        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(repo.GetContext()));

        //    // Bij initialisatie van het systeem wordt Admin aangemaakt
        //    if (!roleManager.RoleExists("Admin"))
        //    {

        //        // Aanmaken van de Admin role
        //        var role = new IdentityRole();
        //        role.Name = "Admin";
        //        roleManager.Create(role);

        //        // Administrator aanmaken

        //        var user = new ApplicationUser();
        //        user.UserName = "AdminQwerty@mail.com";
        //        user.Email = "AdminQwerty@mail.com";

        //        string userPWD = "Azerty123!";

        //        var chkUser = this.Create(user, userPWD);

        //        // Administrator de rol van Admin toewijzen
        //        if (chkUser.Succeeded)
        //        {
        //            var result1 = this.AddToRole(user.Id, "Admin");
        //        }
        //    }

        //    // Manager role aanmaken    
        //    if (!roleManager.RoleExists("Manager"))
        //    {
        //        var role = new IdentityRole();
        //        role.Name = "Manager";
        //        roleManager.Create(role);
        //    }

        //    // Emloyee role aanmaken    
        //    if (!roleManager.RoleExists("Employee"))
        //    {
        //        var role = new IdentityRole();
        //        role.Name = "Employee";
        //        roleManager.Create(role);

        //    }
        //}

        // Een lijst van beschikbare roles ophalen
        public IList<IdentityRole> GetRoles() => ((IdentityRepository)Store).ReadRoles();

        // Alle users ophalen
        public IEnumerable<ApplicationUser> GetUsers() => ((IdentityRepository)Store).ReadUsers();

        // Een specifieke user ophalen
        public ApplicationUser GetUser(string id) => ((IdentityRepository)Store).ReadUser(id);

        // Een user verwijderen obv. de id
        public void RemoveUser(string id) => ((IdentityRepository)Store).DeleteUser(id);

        // Nieuwe gebruikers aanmaken MET hun role
        public async Task<IdentityResult> CreateUserWithRoleAsync(ApplicationUser user,
            string pwd, string role)
        {
            var t = await this.CreateAsync(user, pwd);
            this.AddToRole(user.Id, role);
            return t;
        }

        ////inladen vanuit json formaat
        public void AddApplicationGebruikers(string filePath)
        {
            _gebruikerMgr = new GebruikerManager();

            //sourceUrl /relatief path
            StreamReader r = new StreamReader(filePath);
            string json = r.ReadToEnd();

            dynamic users = JsonConvert.DeserializeObject(json);

            foreach (var item in users.records)
            {
                ApplicationUser gebruiker = new ApplicationUser()
                {
                    UserName = item.Username,
                    VoorNaam = item.Voornaam,
                    AchterNaam = item.Achternaam,
                    Email = item.email,
                    Geboortedatum = item.Geboortedatum
                };
                string passw = item.Password;
                CreateUserWithRoleAsync(gebruiker, passw, "Admin");

                // Er wordt een aparte Gebruiker klasse gebruikt om objecte te linken
                _gebruikerMgr.AddGebruiker(gebruiker.UserName, gebruiker.Id, gebruiker.AchterNaam, gebruiker.UserName);
            }
        }
    }
}