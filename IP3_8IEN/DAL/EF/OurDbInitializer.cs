using System.Collections.Generic;

using IP_8IEN.BL.Domain.Data;
using Newtonsoft.Json;
using System.IO;
using System.Data.Entity;

using IP_8IEN.BL;
using System.Collections.ObjectModel;
using System.Linq;

namespace IP_8IEN.DAL.EF
{
    class OurDbInitializer : DropCreateDatabaseAlways<OurDbContext>
    //DropCreateDatabaseIfModelChanges
    //DropCreateDatabaseAlways
    {

        protected override void Seed(OurDbContext context)
        {
            base.Seed(context);
            
            //DataManager aanspreken met locatie /url
            //DataManager dm = new DataManager();

            /*StreamReader r = new StreamReader($"c:\\Users\\Nathan\\documents\\visual studio 2015\\Projects\\IP3_8IEN\\BL\\textgaindump.json");
            string json = r.ReadToEnd();
            List<Message> messages = new List<Message>();

            dynamic tweets = JsonConvert.DeserializeObject(json);

            string voornaam = null;
            string achternaam = null;

            string word1 = null;
            string word2 = null;
            string word3 = null;
            string word4 = null;
            string word5 = null;

            string mention1 = null;
            string mention2 = null;
            string mention3 = null;
            string mention4 = null;
            string mention5 = null;

            string url1 = null;
            string url2 = null;

            string[] words = { word1, word2, word3, word4, word5 };
            string[] mentions = { mention1, mention2, mention3, mention4, mention5 };
            string[] urls = { url1, url2 };

            foreach (var item in tweets.records)
            {
                for (int i = 0; i <= item.words.Count - 1 && i <= 4; i++)
                {
                    words[i] = item.words[i];
                }

                for (int i = 0; i <= item.mentions.Count - 1 && i <= 4; i++)
                {
                    mentions[i] = item.mentions[i];
                }

                for (int i = 0; i <= item.urls.Count - 1 && i <= 1; i++)
                {
                    urls[i] = item.urls[i];
                }

                Message tweet = new Message()
                {
                    Source = item.source,
                    Id = item.id,
                    UserId = item.user_id,
                    Geo = item.geo,
                    Retweet = item.retweet,
                    Date = item.date,

                    Word1 = words[0],
                    Word2 = words[1],
                    Word3 = words[2],
                    Word4 = words[3],
                    Word5 = words[4],

                    SentimentPos = item.sentiment[0],
                    SentimentNeg = item.sentiment[1],

                    Mention1 = mentions[0],
                    Mention2 = mentions[1],
                    Mention3 = mentions[2],
                    Mention4 = mentions[3],
                    Mention5 = mentions[4],

                    Url1 = urls[0],
                    Url2 = urls[1],

                    SubjectMessages = new Collection<SubjectMessage>()
                };
                context.Messages.Add(tweet);

                voornaam = item.politician[0];
                achternaam = item.politician[1];

                Persoon persoon;

                bool ifExists = context.Personen.Any(x => x.Voornaam == voornaam
                  && x.Achternaam == achternaam);

                if (ifExists == true)
                {
                    persoon = context.Personen.FirstOrDefault(x => x.Voornaam == voornaam
                        && x.Achternaam == achternaam);
                } else
                {
                    persoon = new Persoon()
                    {
                        Voornaam = voornaam,
                        Achternaam = achternaam,
                        Beschrijving = "Desc",
                        Twitter = "twitterurl",
                        SubjectMessages = new Collection<SubjectMessage>()
                    };
                    context.Personen.Add(persoon);
                }

                SubjectMessage subjMess = new SubjectMessage()
                {
                    Msg = tweet,
                    Onderwrp = persoon
                };
                
                tweet.SubjectMessages.Add(subjMess);
                persoon.SubjectMessages.Add(subjMess);
                context.SaveChanges();
            }*/
            
        }
    }
}
