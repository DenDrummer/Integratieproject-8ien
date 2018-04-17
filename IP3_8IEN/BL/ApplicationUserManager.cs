using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using IP_8IEN.DAL.EF;
using IP_8IEN.DAL;
using IP_8IEN.BL.Domain;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IP_8IEN.BL 
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        //1 apr 2018 : Stephane

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
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Role bij een gebruiker toevoegen
            CreateRolesandUsers();
        }

        // Roles in ASP.Identity
        private void CreateRolesandUsers()
        {
            // Context moet opgehaald worden uit de repository!
            // ApplicationDbContext context = (ApplicationDbContext)(((IdentityRepository)Store).Context);
            IdentityRepository repo = new IdentityRepository();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(repo.GetContext()));

            // Bij initialisatie van het systeem wordt Admin aangemaakt
            if (!roleManager.RoleExists("Admin"))
            {

                // Aanmaken van de Admin role
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // Administrator aanmaken

                var user = new ApplicationUser();
                user.UserName = "AdminQwerty";
                user.Email = "AdminQwerty@mail.com";

                string userPWD = "Azerty123.";

                var chkUser = this.Create(user, userPWD);

                // Administrator de rol van Admin toewijzen
                if (chkUser.Succeeded)
                {
                    var result1 = this.AddToRole(user.Id, "Admin");
                }
            }

            // Manager role aanmaken    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // Emloyee role aanmaken    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

            }
        }

        // Een lijst van beschikbare roles ophalen
        public IList<IdentityRole> GetRoles()
        {
            return ((IdentityRepository)Store).ReadRoles();
        }

        // Alle users ophalen
        public IEnumerable<ApplicationUser> GetUsers()
        {
            return ((IdentityRepository)Store).ReadUsers();
        }

        // Een specifieke user ophalen
        public ApplicationUser GetUser(string id)
        {
            return ((IdentityRepository)Store).ReadUser(id);
        }

        // Een user verwijderen obv. de id
        public void RemoveUser(string id)
        {
            ((IdentityRepository)Store).DeleteUser(id);
        }

        // Nieuwe gebruikers aanmaken MET hun role
        public async Task<IdentityResult> CreateUserWithRoleAsync(ApplicationUser user,
            string pwd, string role)
        {
            var t = await this.CreateAsync(user, pwd);
            this.AddToRole(user.Id, role);
            return t;
        }
    }
}
