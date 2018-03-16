using System;
using System.Collections.Generic;
using IP_8IEN.BL.Domain.Data;
using IP_8IEN.DAL.EF;

namespace IP_8IEN.DAL
{
    public class MessageRepository : IMessageRepository
    {
        private OurDbContext ctx;

        public MessageRepository()
        {
            ctx = new OurDbContext();

            ctx.Database.Initialize(true);
        }

        public List<Message> AddingMessages(List<Message> messages)
        {
            foreach (var item in messages)
            {
                ctx.Messages.Add(item);
            }
            ctx.SaveChanges();
            return messages;
        }

        public Message CreateMessage(Message message)
        {
            ctx.Messages.Add(message);
            ctx.SaveChanges();

            return message; // 'MessageNumber' should be created by the database?
        }

        public Message ReadMessage(int messageId)
        {
            Message message = ctx.Messages.Find(messageId);
            return message;
        }

        public IEnumerable<Message> ReadMessages()
        {
            throw new NotImplementedException();
        }
    }
}
