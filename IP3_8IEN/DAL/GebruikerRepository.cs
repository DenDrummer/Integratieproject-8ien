using System.Collections.Generic;
using IP3_8IEN.BL.Domain.Gebruikers;
using IP3_8IEN.DAL.EF;
using System.Linq;
using System.Data.Entity;

namespace IP3_8IEN.DAL
{
    public class GebruikerRepository : IGebruikerRepository
    {
        private OurDbContext ctx;
        public bool isUoW;

        public GebruikerRepository()
        {
            ctx = new OurDbContext();
            //isUoW = false;
            ctx.Database.Initialize(false);
        }

        public GebruikerRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
            //isUoW = true;
        }

        public bool IsUnitofWork()
            => isUoW;

        public void SetUnitofWork(bool UoW) => isUoW = UoW;

        public void AddingAlertInstelling(AlertInstelling alertinstelling)
        {
            ctx.AlertInstellingen.Add(alertinstelling);
            ctx.SaveChanges();
        }

        public void AddingGebruiker(Gebruiker gebruiker)
        {
            ctx.Gebruikers.Add(gebruiker);
            ctx.SaveChanges();
        }

        public Gebruiker ReadGebruiker(string userId)
        {
            IEnumerable<Gebruiker> users = ctx.Gebruikers.Include("Dashboards").ToList();
            return users.FirstOrDefault(u => u.GebruikerId == userId);
        }

        //Deze moet nog ge-update worden
        public void DeleteGebruiker(Gebruiker gebruiker)
        {
            if (gebruiker != null)
            {
                ctx.Gebruikers.Remove(gebruiker);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<Gebruiker> ReadGebruikers()
            => ctx.Gebruikers.ToList();

        public IEnumerable<Gebruiker> ReadGebruikersWithDashbord()
            => ctx.Gebruikers.Include("Dashboards").Include("Dashboards.TileZones").ToList();

        public AlertInstelling ReadAlertInstelling(int alertInstellingId) => ctx.AlertInstellingen.Find(alertInstellingId);

        public void AddingAlert(Alert alert)
        {
            ctx.Alerts.Add(alert);
            ctx.SaveChanges();
        }

        public IEnumerable<Alert> ReadAlerts()
            => ctx.Alerts.ToList();

        public void UpdateAlertInstelling(AlertInstelling alertInstelling)
            => ctx.SaveChanges();

        public IEnumerable<Gebruiker> ReadGebruikersWithAlertInstellingen()
            => ctx.Gebruikers
            .Include("AlertInstellingen")
            .Include("AlertInstellingen.Alerts");

        public Alert ReadAlert(int alertId)
            => ctx.Alerts.Find(alertId);

        public IEnumerable<ValueFluctuation> ReadValueFluctuations()
        {
            IEnumerable<ValueFluctuation> valueFluctuations = ctx.Fluctuations.Include("Alerts").Include("Gebruiker").Include("Onderwerp");
            return valueFluctuations;
        }

        public IEnumerable<HogerLager> ReadHogerLagers()
            => ctx.HogerLagers
            .Include("Onderwerp")
            .Include("Onderwerp2")
            .Include("Gebruiker")
            .Include("Onderwerp");

        public IEnumerable<PositiefNegatief> ReadPositiefNegatiefs()
            => ctx.PositiefNegatiefs
            .Include("Alerts")
            .Include("Gebruiker")
            .Include("Onderwerp");

        public void UpdateGebruiker(Gebruiker gebruiker)
        {
            ctx.Entry(gebruiker).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public IEnumerable<Gebruiker> ReadUsers()
            => ctx.Gebruikers.ToList();
    }
}