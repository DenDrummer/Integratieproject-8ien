using System;
using System.Collections.Generic;

using IP_8IEN.BL.Domain.Data;
using IP_8IEN.DAL.EF;
using System.Linq;

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
            //return ctx.Personen;
            return ctx.Personen.ToList<Persoon>();
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
        public void UdateOnderwerp(Onderwerp onderwerp)
        {
            ctx.SaveChanges();
        }
    }
}
