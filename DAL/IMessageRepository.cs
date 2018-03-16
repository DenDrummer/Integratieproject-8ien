using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IP_8IEN.BL.Domain.Data;

namespace IP_8IEN.DAL
{
    public interface IMessageRepository
    {
        IEnumerable<Message> ReadMessages();
        List<Message> AddingMessages(List<Message> messages);
        Message CreateMessage(Message message);
        Message ReadMessage(int messageId);
    }
}
