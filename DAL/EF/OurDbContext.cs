using System.Data.Entity;

namespace IP_8IEN.DAL.EF
{
    class OurDbContext : DbContext
    {
        public OurDbContext() : base("OurDB_EFCodeFirst")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }
    }
}
