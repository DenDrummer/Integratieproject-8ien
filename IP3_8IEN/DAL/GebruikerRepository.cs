using System;
using System.Collections.Generic;
using IP3_8IEN.BL.Domain.Gebruikers;
using IP3_8IEN.DAL.EF;
using System.Linq;

namespace IP3_8IEN.DAL
{
    public class GebruikerRepository : IGebruikerRepository
    {
        private OurDbContext ctx;
        public bool isUoW;

        public GebruikerRepository()
        {
            ctx = new OurDbContext();
            isUoW = false;
            ctx.Database.Initialize(false);
        }

        public GebruikerRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
            isUoW = true;
        }

        public bool isUnitofWork()
        {
            return isUoW;
        }

        public void setUnitofWork(bool UoW)
        {
            isUoW = UoW;
        }

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

        public Gebruiker FindGebruiker(int userId)
        {
            Gebruiker user = ctx.Gebruikers.Find(userId);
            return user;
        }

        public IEnumerable<Gebruiker> ReadGebruikers()
        {
            IEnumerable<Gebruiker> gebruikers = ctx.Gebruikers.ToList<Gebruiker>();
            return gebruikers;
        }

        public AlertInstelling ReadAlertInstelling(int alertInstellingId)
        {
            AlertInstelling alertInstelling = ctx.AlertInstellingen.Find(alertInstellingId);
            return alertInstelling;
        }

        public void AddingAlert(Alert alert)
        {
            ctx.Alerts.Add(alert);
            ctx.SaveChanges();
        }

        public IEnumerable<Alert> ReadAlerts()
        {
            return ctx.Alerts.ToList<Alert>();
        }

        public void UpdateAlertInstelling(AlertInstelling alertInstelling)
        {
            ctx.SaveChanges();
        }
    }
}
