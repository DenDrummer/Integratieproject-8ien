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

        public bool IsUnitofWork() => isUoW;

        public void SetUnitofWork(bool UoW) => isUoW = UoW;

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
        public IEnumerable<Persoon> ReadPersonenOnly() => ctx.Personen.ToList();
        public IEnumerable<Hashtag> ReadHashtags() => ctx.Hashtags;
        public IEnumerable<Hashtag> ReadHashtagsWithSubjMsgs() => ctx.Hashtags.Include("SubjectMessages");

        public IEnumerable<Onderwerp> ReadSubjects() => ctx.Onderwerpen;

        public IEnumerable<Organisatie> ReadOrganisaties() => ctx.Organisaties.ToList();

        public void AddingTewerkstelling(Tewerkstelling tewerkstelling)
        {
            ctx.Tewerkstellingen.Add(tewerkstelling);
            ctx.SaveChanges();
        }
        public IEnumerable<SubjectMessage> ReadSubjectMessages() => ctx.SubjectMessages.ToList();

        public IEnumerable<SubjectMessage> ReadSubjectMessagesWithHashtags()
        {
            return ctx.SubjectMessages.Include("Hashtag").ToList();
        }

        public IEnumerable<Message> ReadMessages() => ctx.Messages.ToList();

        public void UpdateOnderwerp(Onderwerp onderwerp) => ctx.SaveChanges();

        public void UpdateMessage() => ctx.SaveChanges();

        public Persoon ReadPersoon(int persoonId)
        {
            IEnumerable<Persoon> personen = ctx.Personen.Include("Tewerkstellingen")
                .Include("Tewerkstellingen.Organisatie");//.Find(persoonId);
            return personen.FirstOrDefault(p => p.OnderwerpId == persoonId);
        }
        public Persoon ReadPersoonWithSbjctMsg(int persoonId)
        {
            IEnumerable<Persoon> personen = ctx.Personen.Include("Tewerkstellingen")
                .Include("Tewerkstellingen.Organisatie")
                .Include("SubjectMessages")
                .Include("SubjectMessages.Msg");//.Find(persoonId);
            return personen.FirstOrDefault(p => p.OnderwerpId == persoonId);
        }
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

        //public IEnumerable<Message> ReadMessagesWithOrg()
        //{
        //    IEnumerable<Message> messages = ctx.Messages.Include("SubjectMessages").Include("SubjectMessages.Persoon").
        //        Include("SubjectMessages.Persoon.Tewerkstellingen").Include("SubjectMessages.Persoon.Tewerkstellingen");
        //    return messages;
        //}

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

        public void EditPersoon(Persoon persoon)
        {
            ctx.Entry(persoon).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public Persoon ReadPersoon(string naam)
        {
            Persoon persoon = ctx.Personen.Include("SubjectMessages").Include("SubjectMessages.Msg").Include("Tewerkstellingen").Where(p => p.Naam == naam).FirstOrDefault();
            return persoon;
        }
        public Persoon ReadPersoonWithTewerkstelling(string naam) {
            return ctx.Personen.Include("Tewerkstellingen").Include("Tewerkstellingen.Organisatie").FirstOrDefault(p => p.Naam == naam);
        }
        public Persoon ReadPersoonWithTewerkstelling(int id)
        {
            return ctx.Personen.Include("Tewerkstellingen").Include("Tewerkstellingen.Organisatie").FirstOrDefault(p => p.OnderwerpId == id);
        }
        public IEnumerable<Persoon> ReadPersonenWithTewerkstelling()
        {
            return ctx.Personen.Include("Tewerkstellingen").Include("Tewerkstellingen.Organisatie").Include("SubjectMessages").ToList();
        }

        public void UpdateHashtag(Hashtag hashtag)
        {
            ctx.Entry(hashtag).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void CreateTheme(Thema theme)
        {
            ctx.Themas.Add(theme);
            ctx.SaveChanges();
        }
        public IEnumerable<Thema> ReadThemas()
        {
            return ctx.Themas.ToList();
        }
        public void UpdateTheme(Thema theme)
        {
            ctx.Entry(theme).State = EntityState.Modified;
            ctx.SaveChanges();
        }
        public Thema ReadThemas(int id)
        {
            return ctx.Themas.Find(id);
        }
    }
}