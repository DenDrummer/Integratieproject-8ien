using System;
using System.Data.Entity;
using IP_8IEN.BL.Domain.Data;
using IP_8IEN.BL.Domain.Gebruikers;

namespace IP_8IEN.DAL.EF
{
    [DbConfigurationType(typeof(OurDbConfiguration))]
    internal class OurDbContext : DbContext
    {
        //private readonly bool delaySave;
        //DelaySave zorgt ervoor dat de gewone SaveChanges niet uitgevoerd wordt
        // indien deze boolean op true staat. 
        /*public OurDbContext(bool unitOfWorkPresent = false) : base("OurDB_EFCodeFirst")
        {
            Database.SetInitializer<OurDbContext>(new OurDbInitializer());
            delaySave = unitOfWorkPresent;
        }*/

        public OurDbContext() : base("OurDB_EFCodeFirst")
        {
            Database.SetInitializer<OurDbContext>(new OurDbInitializer());
        }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<AlertInstelling>().HasRequired(g => g.Gebruiker).WithMany(a => a.AlertInstellingen).HasForeignKey<int>(g => g.GebruikerId);
            //modelBuilder.Entity<AlertInstelling>().HasRequired(o => o.Onderwerp).WithMany(a => a.AlertInstellingen).HasForeignKey<int>(o => o.OnderwerpId);
            //modelBuilder.Entity<Alert>().HasRequired(a => a.AlertInstelling).WithMany(a => a.Alerts).HasForeignKey<int>(a => a.AlertId);

            modelBuilder.Entity<Message>().HasKey(m => m.MessageId);
        }



        /*public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<AlertInstelling> AlertInstellingen { get; set; }
        public DbSet<Onderwerp> Onderwerpen { get; set; }

        public override int SaveChanges()
        {
            if (delaySave) return -1;
            return base.SaveChanges();
        }

        internal int CommitChanges()
        {
            if (delaySave)
            {
                return base.SaveChanges();
            }
            throw new InvalidOperationException("No UnitOfWork present, use SaveChanges instead");
        }*/

    }
}
