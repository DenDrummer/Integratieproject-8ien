using System.Data.Entity;
using IP3_8IEN.BL.Domain.Data;
using IP3_8IEN.BL.Domain.Gebruikers;
using System;
using IP3_8IEN.BL.Domain.Dashboard;
using IP3_8IEN.BL.Domain.Globalization;

namespace IP3_8IEN.DAL.EF
{
    [DbConfigurationType(typeof(OurDbConfiguration))]
    internal class OurDbContext : DbContext
    {
        private readonly bool delaySave;


        public OurDbContext(bool unitOfWorkPresent = false) : base("OurDB_EFCodeFirst")
        {
            //Database.SetInitializer<OurDbContext>(new OurDbInitializer());
            delaySave = unitOfWorkPresent;
        }

        //16 mrt 2018 : Stephane
        public DbSet<Message> Messages { get; set; }
        //27 mrt 2018 : Stephane
        public DbSet<SubjectMessage> SubjectMessages { get; set; }
        public DbSet<Persoon> Personen { get; set; }
        //28 mrt 2018 : Stephane
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Onderwerp> Onderwerpen { get; set; }
        //30 mrt 2018 : Stephane
        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<AlertInstelling> AlertInstellingen { get; set; }
        //31 mrt 2018 : Stephane
        public DbSet<Alert> Alerts { get; set; }
        //2 apr 2018 : Stephane
        public DbSet<Organisatie> Organisaties { get; set; }
        public DbSet<Tewerkstelling> Tewerkstellingen { get; set; }
        //5 apr 2018 : Stephane
        public DbSet<DashItem> DashItems { get; set; }
        public DbSet<Cijfer> Cijfers { get; set; }
        public DbSet<Kruising> Kruisingen { get; set; }
        public DbSet<Vergelijking> Vergelijkingen { get; set; }

        //4 mei 2018 : Victor
        public DbSet<ValueFluctuation> Fluctuations { get; set; }
        public DbSet<PositiefNegatief> PositiefNegatiefs { get; set; }
        public DbSet<HogerLager> HogerLagers { get; set; }

        //10 mei 2018 : Stephane
        public DbSet<Dashbord> Dashbords { get; set; }
        public DbSet<TileZone> TileZones { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<GraphData> Graphs { get; set; }
        public DbSet<GraphData2> Graphs2 { get; set; }

        public DbSet<Thema> Themas { get; set; }

        #region Globalization
        public DbSet<GlobalizationPlatform> GlobalizationPlatforms { get; set; }
        public DbSet<GlobalizationObject> GlobalizationItems { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //hier komt 'Fluent api' als je dat nodig zou hebben
        }

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
        }
    }
}