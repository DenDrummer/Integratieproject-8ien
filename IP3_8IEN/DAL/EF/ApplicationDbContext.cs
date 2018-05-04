using IP_8IEN.BL.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IP_8IEN.DAL.EF
{
    // Dit stond vroeger in UI_MVC_S -> Bad Practice
    // Toegang tot databank hoort thuis in DAL
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("OurDB_EFCodeFirst", throwIfV1Schema: false)
        {

        }

        /*
         * DbSet Users werd reeds voorzien door IdentityDbContext
        public System.Data.Entity.DbSet<SC.BL.Domain.ApplicationUser> ApplicationUsers { get; set; }
        */
    }
}
