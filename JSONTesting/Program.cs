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
            dynamic tweets = JsonConvert.DeserializeObject(json);
            List<Message> msgs = new List<Message>();
            foreach (var item in tweets.records)
            {
                Message msg = new Message(
                    msgs.Count,
                    item.source,
                    item.id,
                    item.user_id,
                    item.geo,
                    item.mentions.ToObject<List<string>>(),
                    item.retweet,
                    item.date,
                    item.words.ToObject<List<string>>(),
                    item.sentiment.ToObject<List<int>>(),
                    item.hashtags.ToObject<List<string>>(),
                    item.urls.ToObject<List<string>>(),
                    item.politician.ToObject<List<string>>()));
                Console.WriteLine(msg.MessageId);
                Console.WriteLine(msg.Source);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
