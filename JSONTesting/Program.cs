using IP_8IEN.BL.Domain.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader r = new StreamReader($"~\\..\\..\\..\\textgaindump.json");
            string json = r.ReadToEnd();
            dynamic records = JsonConvert.DeserializeObject(json);
            List<Message> msgs = new List<Message>();
            int i = 0;
            foreach (dynamic record in records)
            {
                //msgs.Add(new Message(i));
                i++;
            }
            //List<Message> msgs =  JsonConvert.DeserializeObject<List<Message>>(json);
        }
    }
}
