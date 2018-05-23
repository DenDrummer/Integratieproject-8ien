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

        public IEnumerable<Organisatie> ReadOrganisaties() => ctx.Organisaties.ToList();

        public void AddingTewerkstelling(Tewerkstelling tewerkstelling)
        {
            ctx.Tewerkstellingen.Add(tewerkstelling);
            ctx.SaveChanges();
        }
        public IEnumerable<SubjectMessage> ReadSubjectMessages() => ctx.SubjectMessages.ToList();

        public IEnumerable<Message> ReadMessages() => ctx.Messages.ToList();

        public void UdateOnderwerp(Onderwerp onderwerp) => ctx.SaveChanges();

        public void UpdateMessage() => ctx.SaveChanges();

        public Persoon ReadPersoon(int persoonId)
        {
            IEnumerable<Persoon> personen = ctx.Personen.Include("Tewerkstellingen").Include("Tewerkstellingen.Organisatie");//.Find(persoonId);
            return personen.FirstOrDefault(p => p.OnderwerpId == persoonId);
        }
        public IEnumerable<Tewerkstelling> ReadTewerkstellingen() => ctx.Tewerkstellingen.ToList();

        public IEnumerable<Message> ReadMessages(bool subjM)
        {
            if (subjM)
            {
                IEnumerable<Message> messages = ctx.Messages.Include("SubjectMessages").Include("SubjectMessages.Persoon");
                return messages;
            }
            else
            {
                return ReadMessages();
            }
        }
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

    }
}