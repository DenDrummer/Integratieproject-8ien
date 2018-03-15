using IP_8IEN.BL.Domain.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Message msg = JsonConvert.DeserializeObject<Message>("?"); //vervang met link naar json?
        }
    }
}
