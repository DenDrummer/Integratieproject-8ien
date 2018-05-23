using System.Collections.Generic;
using IP_8IEN.BL.Domain.Gebruikers;
using IP_8IEN.DAL.EF;
using System.Linq;

namespace IP_8IEN.DAL
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

        public bool isUnitofWork() => isUoW;

        public void setUnitofWork(bool UoW) => isUoW = UoW;

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
            Gebruiker gebruiker = users.FirstOrDefault(u => u.GebruikerId == userId);
            return gebruiker;
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
        {
            IEnumerable<Gebruiker> gebruikers = ctx.Gebruikers.ToList<Gebruiker>();
            return gebruikers;
        }

        public AlertInstelling ReadAlertInstelling(int alertInstellingId) => ctx.AlertInstellingen.Find(alertInstellingId);

        public void AddingAlert(Alert alert)
        {
            ctx.Alerts.Add(alert);
            ctx.SaveChanges();
        }

        public IEnumerable<Alert> ReadAlerts() => ctx.Alerts.ToList();

        public void UpdateAlertInstelling(AlertInstelling alertInstelling) => ctx.SaveChanges();

        public IEnumerable<Gebruiker> ReadGebruikersWithAlertInstellingen() => ctx.Gebruikers.Include("AlertInstellingen").Include("AlertInstellingen.Alerts");

        public void UpdateAlertInstelling(AlertInstelling alertInstelling)
        {
            ctx.SaveChanges();
        }
        public IEnumerable<Gebruiker> ReadGebruikersWithAlertInstellingen()
        {
            IEnumerable<Gebruiker> gebruikers = ctx.Gebruikers.Include("AlertInstellingen").Include("AlertInstellingen.Alerts");
            return gebruikers;
        }
        public Alert ReadAlert(int alertId)
        {
            Alert alert = ctx.Alerts.Find(alertId);
            return alert;
        }
        public IEnumerable<ValueFluctuation> ReadValueFluctuations()
        {
            IEnumerable<ValueFluctuation> valueFluctuations = ctx.Fluctuations.Include("Alerts").Include("Gebruiker").Include("Onderwerp");
            return valueFluctuations;
        }

        public IEnumerable<HogerLager> ReadHogerLagers()
        {
            IEnumerable<HogerLager> hogerLagers = ctx.HogerLagers.Include("Onderwerp").Include("Onderwerp2").Include("Gebruiker").Include("Onderwerp");
            return hogerLagers;
        }

        public IEnumerable<PositiefNegatief> ReadPositiefNegatiefs()
        {
            IEnumerable<PositiefNegatief> positiefNegatiefs = ctx.PositiefNegatiefs.Include("Alerts").Include("Gebruiker").Include("Onderwerp");
            return positiefNegatiefs;
        }

        public void UpdateGebruiker(Gebruiker gebruiker)
        {
            ctx.Entry(gebruiker).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}