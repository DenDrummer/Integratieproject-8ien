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
            int i = 0;
            foreach (var item in tweets.records)
            {
                Message msg = new Message();
                msg.MessageId = 0;
                msg.Source = item.source;
                msg.Id = item.id;
                msg.UserId = item.user_id;
                msg.Geo = item.geo;
                msg.Mentions = item.mentions.ToObject<List<string>>();
                msg.Retweet = item.retweet;
                msg.Date = item.date;
                msg.Words = item.words.ToObject<List<string>>();
                msg.Sentiment = item.sentiment.ToObject<List<double>>();
                msg.Hashtags = item.hashtags.ToObject<List<string>>();
                msg.Urls = item.urls.ToObject<List<string>>();
                msg.Politician = item.politician.ToObject<List<string>>();
                i++;
                Console.WriteLine(msg.MessageId);
                Console.WriteLine(msg.Source);
                Console.WriteLine(msg.Id);
                Console.WriteLine(msg.UserId);
                Console.WriteLine(msg.Geo);
                Console.WriteLine("Mentions:");
                foreach (string mention in msg.Mentions)
                {
                    Console.WriteLine($"\t{mention}");
                }
                Console.WriteLine(msg.Retweet);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.ReadKey();
            }
        }
    }
}
