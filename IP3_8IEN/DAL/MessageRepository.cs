using System.Collections.Generic;

using IP3_8IEN.BL.Domain.Data;
using IP3_8IEN.DAL.EF;
using System.Linq;
using System.Data.Entity;

namespace IP3_8IEN.DAL
{
    public class MessageRepository : IMessageRepository
    {
        //Dit is de repo voor de 'Data' package

        private OurDbContext ctx;
        public bool isUoW;

        public MessageRepository()
        {
            ctx = new OurDbContext();
            ctx.Database.Initialize(false);
        }

        public MessageRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
        }

        public bool isUnitofWork() => isUoW;

        public void setUnitofWork(bool UoW) => isUoW = UoW;

        public void AddingMessage(Message message)
        {
            ctx.Messages.Add(message);
            ctx.SaveChanges();
        }

        public void AddOnderwerp(Onderwerp onderwerp)
        {
            ctx.Onderwerpen.Add(onderwerp);
            ctx.SaveChanges();
        }

        public void AddSubjectMsg(SubjectMessage subjMsg)
        {
            ctx.SubjectMessages.Add(subjMsg);
            ctx.SaveChanges();
        }

        public IEnumerable<Persoon> ReadPersonen() => ctx.Personen.Include("Tewerkstellingen").Include("Tewerkstellingen.Organisatie").ToList();

        public IEnumerable<Hashtag> ReadHashtags() => ctx.Hashtags;

        public IEnumerable<Onderwerp> ReadSubjects() => ctx.Onderwerpen;

        public void AddingTewerkstelling(Tewerkstelling tewerkstelling)
        {
            ctx.Tewerkstellingen.Add(tewerkstelling);
            ctx.SaveChanges();
        }
        public IEnumerable<SubjectMessage> ReadSubjectMessages() => ctx.SubjectMessages.ToList();

        public IEnumerable<Message> ReadMessages() => ctx.Messages.ToList();

        public void UpdateOnderwerp(Onderwerp onderwerp) => ctx.SaveChanges();

        public void UpdateMessage() => ctx.SaveChanges();
        public IEnumerable<Tewerkstelling> ReadTewerkstellingen() => ctx.Tewerkstellingen.ToList();

        public IEnumerable<Message> ReadMessages(bool subjM)
        {
            if (subjM)
            {
                IEnumerable<Message> messages = ctx.Messages.Include("SubjectMessages").Include("SubjectMessages.Persoon").Include("SubjectMessages.Persoon.Tewerkstellingen");
                return messages;
            }
            else
            {
                return ReadMessages();
            }
        }

        #region Organisaties
        public Organisatie ReadOrganisatie(int organisatieId)
        {
            IEnumerable<Organisatie> organisaties = ctx.Organisaties.Include("Tewerkstellingen").Include("Tewerkstellingen.Persoon");
            return organisaties.FirstOrDefault(o => o.OnderwerpId == organisatieId);
        }

        public void EditOrganisation(Organisatie organisatie)
        {
            ctx.Entry(organisatie).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public IEnumerable<Organisatie> ReadOrganisaties() => ctx.Organisaties.ToList();
        #endregion

        #region Personen
        public void EditPersoon(Persoon persoon)
        {
            ctx.Entry(persoon).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public Persoon ReadPersoon(string naam)
        {
            Persoon persoon = ctx.Personen.Include("SubjectMessages").Include("SubjectMessages.Msg").Include("Tewerkstellingen").Where(p => p.Naam.Equals(naam)).FirstOrDefault();
            return persoon;
        }

        public Persoon ReadPersoon(int persoonId)
        {
            IEnumerable<Persoon> personen = ctx.Personen.Include("Tewerkstellingen").Include("Tewerkstellingen.Organisatie");//.Find(persoonId);
            return personen.FirstOrDefault(p => p.OnderwerpId == persoonId);
        }
        #endregion

        #region themas
        public List<Thema> ReadThemas()
        {
            return ctx.Themas.ToList();
        }

        public void EditThema(Thema thema)
        {
            ctx.Entry(thema).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public Thema ReadThema(int onderwerpId)
        {
            Thema thema = ctx.Themas.Include("SubjectMessages").Include("SubjectMessages.Msg").Where(t => t.OnderwerpId == onderwerpId).FirstOrDefault();
            throw new System.NotImplementedException();
        }
        #endregion
        public Persoon ReadPersoonWithTewerkstelling(string naam) {
            return ctx.Personen.Include("Tewerkstellingen").Include("Tewerkstellingen.Organisatie").FirstOrDefault(p => p.Naam == naam);
        }
        public Persoon ReadPersoonWithTewerkstelling(int id)
        {
            return ctx.Personen.Include("Tewerkstellingen").Include("Tewerkstellingen.Organisatie").FirstOrDefault(p => p.OnderwerpId == id);
        }
    }
}