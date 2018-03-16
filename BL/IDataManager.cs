using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IP_8IEN.BL.Domain.Data;

namespace IP_8IEN.BL
{
    public interface IDataManager
    {
        void AddMessages(string sourceUrl);
        Message GetMessage(int messageId);
    }
}
