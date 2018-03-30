using IP3_8IEN.BL.Domain.Data;
using Newtonsoft.Json;
using System.IO;

using IP3_8IEN.DAL;
using IP3_8IEN.DAL.EF;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;

namespace IP3_8IEN.BL
{
    public class DataManager : IDataManager
    {
        public readonly IMessageRepository repo;//= new MessageRepository();

        public DataManager()
        {
            repo = new MessageRepository();
        }


        void IDataManager.AddMessages(string sourceUrl)
        {
            //sourceUrl /relatief path
            StreamReader r = new StreamReader(sourceUrl);
            string json = r.ReadToEnd();
            List<Message> messages = new List<Message>();

            dynamic tweets = JsonConvert.DeserializeObject(json);

            //initialisatie van velden voor array
            string word1 = null, word2 = null, word3 = null, word4 = null, word5 = null;
            string mention1 = null, mention2 = null, mention3 = null, mention4 = null, mention5 = null;
            string url1 = null, url2 = null;

            //arrays die we gaan opvullen met arrayobjecten uit de dynamische json objecten
            string[] words = { word1, word2, word3, word4, word5 };
            string[] mentions = { mention1, mention2, mention3, mention4, mention5 };
            string[] urls = { url1, url2 };

            foreach (var item in tweets.records)
            {
                //voorgaande arrays opvullen
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
                repo.AddingMessage(tweet);

                string voornaam = item.politician[0];
                string achternaam = item.politician[1];
                Persoon persoon = AddPersoon(voornaam,achternaam);

                AddSubjectMessage(tweet,persoon);

                foreach (string hashtag in item.hashtags)
                {
                    Hashtag hasht = AddHashtag(hashtag);
                    AddSubjectMessage(tweet, hasht);
                }
            }
        }
        
        public Persoon AddPersoon(string voornaam,string achternaam)
        {
            Persoon persoon;
            IEnumerable<Persoon> personen = repo.ReadPersonen();

            bool ifExists = personen.Any(x => x.Voornaam == voornaam
                  && x.Achternaam == achternaam);

            if (ifExists == true)
            {
                persoon = personen.FirstOrDefault(x => x.Voornaam == voornaam
                    && x.Achternaam == achternaam);
            }
            else
            {
                persoon = new Persoon()
                {
                    Voornaam = voornaam,
                    Achternaam = achternaam,
                    SubjectMessages = new Collection<SubjectMessage>()
                };
                repo.AddOnderwerp(persoon);
            }
            return persoon;
        }

        public Hashtag AddHashtag(string hashtag)
        {
            Hashtag hasht;
            IEnumerable<Hashtag> hashtags = repo.ReadHashtags();

            bool ifExists = hashtags.Any(x => x.HashtagString == hashtag);

            if (ifExists == true)
            {
                hasht = hashtags.FirstOrDefault(x => x.HashtagString == hashtag);
            }
            else
            {
                hasht = new Hashtag()
                {
                    HashtagString = hashtag,
                    SubjectMessages = new Collection<SubjectMessage>()
                };
                repo.AddOnderwerp(hasht);
            }

            return hasht;
        }

        public void AddSubjectMessage(Message msg, Persoon persoon)
        {
            SubjectMessage subjMess = new SubjectMessage()
            {
                Msg = msg,
                Persoon = persoon
            };
            repo.AddSubjectMsg(subjMess);
        }

        public void AddSubjectMessage(Message msg, Hashtag hashtag)
        {
            SubjectMessage subjMess = new SubjectMessage()
            {
                Msg = msg,
                Hashtag = hashtag
            };
            repo.AddSubjectMsg(subjMess);
        }
    }
}

