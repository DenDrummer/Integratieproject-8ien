using System;
using System.Collections.Generic;

using IP3_8IEN.BL.Domain.Data;
using IP3_8IEN.DAL.EF;

namespace IP3_8IEN.DAL
{
    public class MessageRepository : IMessageRepository
    {
        private OurDbContext ctx;

        public MessageRepository()
        {
            ctx = new OurDbContext();
            ctx.Database.Initialize(true);
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
            return ctx.Personen;
        }

        public IEnumerable<Hashtag> ReadHashtags()
        {
            return ctx.Hashtags;
        }
    }
}
