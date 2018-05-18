using IP_8IEN.BL.Domain.Dashboard;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Gebruikers
{
    // Stond vroeger in UI_MVC_S -> Domainklasse hoort in BL.Domain thuis
    // Basis model voor het aanmaken van een nieuwe gebruiker
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        // Extentions 
        public DateTime? Geboortedatum { get; set; }
        public string VoorNaam { get; set; }
        public string AchterNaam { get; set; }

        public ICollection<WeeklyReview> WeeklyReviews { get; set; }
        public ICollection<Dashbord> Dashboards { get; set; }
        public ICollection<AlertInstelling> AlertInstellingen { get; set; }
    }
}
