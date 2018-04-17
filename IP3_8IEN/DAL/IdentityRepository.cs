using IP_8IEN.BL.Domain;
using IP_8IEN.DAL.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace IP_8IEN.DAL
{
    //1 apr 2018 : Stephane

    // Dit stond in UI_MVC_S -> Bad Practice
    // Repositories horen thuis in DAL
    // IdentityRepository geeft toegang tot de Authenticatiegegevens in de DB
    public class IdentityRepository : UserStore<ApplicationUser>
    {
        private static ApplicationDbContext ctx;

        static IdentityRepository() { ctx = new ApplicationDbContext(); }

        public IdentityRepository()
            : base(ctx)
        {

        }

        // Er mogen geen nieuwe Admin's aangemaakt worden -> niet beschikbaar!
        public IList<IdentityRole> ReadRoles()
        {
            return ctx.Roles.Where(u => !u.Name.Contains("Admin")).ToList();
        }

        // Alle gebruikers behalve de administrator ophalen
        public IEnumerable<ApplicationUser> ReadUsers()
        {
            IdentityRole role = ctx.Roles.SingleOrDefault(u => u.Name.Contains("Admin"));
            string adminId = role.Users.First().UserId;

            return ctx.Users.Where(u => !u.Id.Equals(adminId));
            //return ctx.Users.AsEnumerable();
        }

        // Specifieke gebruiker ophalen obv. id
        public ApplicationUser ReadUser(string id)
        {
            return ctx.Users.SingleOrDefault(u => u.Id == id);
        }

        // Gebruiker verwijderen obv. id
        public void DeleteUser(string id)
        {
            ApplicationUser user = ctx.Users.Find(id);
            if (user != null)
            {
                ctx.Users.Remove(user);
                ctx.SaveChanges();
            }
        }

        // DbContext beschikbaar maken voor BL klasses
        public ApplicationDbContext GetContext()
        {
            return ctx;
        }

    }
}
