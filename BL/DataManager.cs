using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IP_8IEN.BL.Domain.Data;
using Newtonsoft.Json;
using System.IO;

using IP_8IEN.DAL;
using IP_8IEN.DAL.EF;

namespace IP_8IEN.BL
{
    public class DataManager : IDataManager
    {
        public readonly IMessageRepository repo = new MessageRepository();

        public DataManager()
        {
            repo = new DAL.MessageRepository();
        }





        void IDataManager.AddMessages(string sourceUrl)
        {
            //sourceUrl
            StreamReader r = new StreamReader($"~\\..\\..\\..\\textgaindump.json");
            string json = r.ReadToEnd();
            List<Message> messages = new List<Message>();

            dynamic tweets = JsonConvert.DeserializeObject(json);


            foreach (var item in tweets.records)
            {
                var message = new Message()
                    {
                        Source = item.source,
                        Id = item.id,
                        UserId = item.user_id,
                        Geo = item.geo,
                        Mentions = item.mentions.ToObject<List<string>>(),
                        Retweet = item.retweet,
                        Date = item.date,
                        Words = item.words.ToObject<List<string>>(),
                        Sentiment = item.sentiment.ToObject<List<int>>(),
                        Hashtags = item.hashtags.ToObject<List<string>>(),
                        Urls = item.urls.ToObject<List<string>>(),
                        Politician = item.politician.ToObject<List<string>>()
                    };
                message.MessageId = messages.Count + 1;
                messages.Add(message

                );
            }
            repo.AddingMessages(messages);
        }

        public Message GetMessage(int messageId)
        {
           return repo.ReadMessage(messageId);
        }

       
    }
}
