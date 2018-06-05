using IP3_8IEN.BL.Domain.Gebruikers;
using IP3_8IEN.DAL.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace IP3_8IEN.DAL
{
    // IdentityRepository geeft toegang tot de Authenticatiegegevens in de DB
    public class IdentityRepository : UserStore<ApplicationUser>
    {
        private static ApplicationDbContext ctx;

        static IdentityRepository() { ctx = new ApplicationDbContext(); }

        public IdentityRepository()
            : base(ctx)
        {

        }

        // Er mogen geen nieuwe SuperAdmin's aangemaakt worden -> niet beschikbaar!
        public IList<IdentityRole> ReadRoles()
            => ctx.Roles.Where(u => !u.Name.Contains("SuperAdmin")).ToList();

        // Alle gebruikers behalve de administrator ophalen
        public IEnumerable<ApplicationUser> ReadUsers()
        {
            //IdentityRole role = ctx.Roles.FirstOrDefault(u => u.Platform.Contains("Admin"));
            //string adminId = role.Users.First().UserId;

            //return ctx.Users.Where(u => !u.Id.Equals(adminId));
            return ctx.Users.ToList(); // AsEnumerable();
        }

        // Specifieke gebruiker ophalen obv. id
        public ApplicationUser ReadUser(string id)
            => ctx.Users.SingleOrDefault(u => u.Id == id);

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
            => ctx;
    }
}