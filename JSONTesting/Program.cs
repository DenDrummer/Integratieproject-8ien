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
                Console.WriteLine("Message Id:");
                Console.WriteLine($"\t{msg.MessageId}");
                Console.WriteLine("Source:");
                Console.WriteLine($"\t{msg.Source}");
                Console.WriteLine("Id:");
                Console.WriteLine($"\t{msg.Id}");
                Console.WriteLine("User Id:");
                Console.WriteLine($"\t{msg.UserId}");
                Console.WriteLine("Geo:");
                Console.WriteLine($"\t{msg.Geo}");
                Console.WriteLine("Mentions:");
                foreach (string mention in msg.Mentions)
                {
                    Console.WriteLine($"\t{mention}");
                }
                Console.WriteLine($"\t{msg.Retweet}");
                Console.WriteLine("Date:");
                Console.WriteLine($"\t{msg.Date}");
                Console.WriteLine("Words:");
                foreach (string word in msg.Words)
                {
                    Console.WriteLine($"\t{word}");
                }
                Console.WriteLine("Sentiment:");
                foreach (double sentiment in msg.Sentiment)
                {
                    Console.WriteLine($"\t{sentiment}");
                }
                Console.WriteLine("Hashtags:");
                foreach (string hashtag in msg.Hashtags)
                {
                    Console.WriteLine($"\t{hashtag}");
                }
                Console.WriteLine("Urls:");
                foreach (string url in msg.Urls)
                {
                    Console.WriteLine($"\t{url}");
                }
                Console.WriteLine("Politician:");
                foreach (string politician in msg.Politician)
                {
                    Console.Write($"\t{politician} ");
                }
                Console.WriteLine();
                Console.WriteLine("-=-=-=-=-=-=-=-=-=-");
                Console.WriteLine();
                msgs.Add(msg);
            }
            //send msgs to 
            Console.ReadKey();
        }
    }
}
