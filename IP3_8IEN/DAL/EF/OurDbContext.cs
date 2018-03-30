using System.Data.Entity;
using IP3_8IEN.BL.Domain.Data;


namespace IP3_8IEN.DAL.EF
{
    [DbConfigurationType(typeof(OurDbConfiguration))]
    internal class OurDbContext : DbContext
    {

        public OurDbContext() : base("OurDB_EFCodeFirst")
        {
            //Database.SetInitializer<OurDbContext>(new OurDbInitializer());
        }

        //16 mrt 2018 : Stephane
        public DbSet<Message> Messages { get; set; }
        //27 mrt 2018 : Stephane
        public DbSet<SubjectMessage> SubjectMessages { get; set; }
        public DbSet<Persoon> Personen { get; set; }
        //28 mrt 2018 : Stephane
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Onderwerp> Onderwerpen { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Message>().HasKey(m => m.MessageId);
        }
    }
}
