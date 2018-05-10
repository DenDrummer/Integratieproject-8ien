using System.Collections.Generic;

using IP_8IEN.BL.Domain.Data;
using IP_8IEN.DAL.EF;
using System.Linq;
using System.Data.Entity;

namespace IP_8IEN.DAL
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

        public bool isUnitofWork()
        {
            return isUoW;
        }

        public void setUnitofWork(bool UoW)
        {
            isUoW = UoW;
        }

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

        public IEnumerable<Persoon> ReadPersonen()
        {
            return ctx.Personen.Include("Tewerkstellingen").Include("Tewerkstellingen.Organisatie").ToList<Persoon>();
            //return ctx.Personen.ToList<Persoon>();
        }

        public IEnumerable<Hashtag> ReadHashtags()
        {
            return ctx.Hashtags;
        }

        public IEnumerable<Onderwerp> ReadSubjects()
        {
            //Onderwerp onderwerp = ctx.Onderwerpen.Find(onderwerpId);
            return ctx.Onderwerpen;
        }

        public IEnumerable<Organisatie> ReadOrganisaties()
        {            
            return ctx.Organisaties.ToList<Organisatie>();
        }

        public void AddingTewerkstelling(Tewerkstelling tewerkstelling)
        {
            ctx.Tewerkstellingen.Add(tewerkstelling);
            ctx.SaveChanges();
        }
        public IEnumerable<SubjectMessage> ReadSubjectMessages()
        {
            return ctx.SubjectMessages.ToList<SubjectMessage>();
        }
        public IEnumerable<Message> ReadMessages()
        {
            return ctx.Messages.ToList<Message>();
        }
        public void UdateOnderwerp(Onderwerp onderwerp)
        {
            ctx.SaveChanges();
        }
        public void UpdateMessage()
        {
            ctx.SaveChanges();
        }
        public Persoon ReadPersoon(int persoonId)
        {
            IEnumerable<Persoon> personen = ctx.Personen.Include("Tewerkstellingen").Include("Tewerkstellingen.Organisatie");//.Find(persoonId);
            return personen.FirstOrDefault(p => p.OnderwerpId == persoonId);
        }
        public IEnumerable<Tewerkstelling> ReadTewerkstellingen()
        {
            return ctx.Tewerkstellingen.ToList<Tewerkstelling>();
        }
        public IEnumerable<Message> ReadMessages(bool subjM)
        {
            if(subjM)
            {
                IEnumerable<Message> messages = ctx.Messages.Include("SubjectMessages").Include("SubjectMessages.Persoon");
                return messages;
            } else
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
