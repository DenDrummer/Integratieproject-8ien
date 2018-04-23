using IP_8IEN.BL.Domain.Data;
using Newtonsoft.Json;
using System.IO;

using IP_8IEN.DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Net;
using System.Web.Script.Serialization;
using System.Web;

namespace IP_8IEN.BL
{
    public class DataManager : IDataManager
    {
        private UnitOfWorkManager uowManager;
        private IMessageRepository repo;//= new MessageRepository();

        // Deze constructor gebruiken we voor operaties binnen de package
        public DataManager()
        {
            //repo = new MessageRepository();
        }

        // We roepen deze constructor aan wanneer we met twee repositories gaan werken
        public DataManager(UnitOfWorkManager uowMgr)
        {
            uowManager = uowMgr;
            repo = new MessageRepository(uowManager.UnitOfWork);
        }

        //httpWebRequest POST naar 'textgain' api --> output doorgegeven aan 'AddMessages'
        public void ApiRequestToJson()
        {
            {
                string url = "http://kdg.textgain.com/query";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Headers.Add("X-API-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Accept = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";

                string json;
                string jsonReturn;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //query opstellen : named parameters
                    json = new JavaScriptSerializer().Serialize(new
                    {
                        //name = "Annick De Ridder",
                        since = "19 Apr 2018 8:00",
                        //until weglaten --> last scraping
                        until = "19 Apr 2018 22:00",
                    });

                    streamWriter.Write(json);
                }

                var serializer = new JsonSerializer();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    jsonReturn = streamReader.ReadToEnd();
                }

                AddMessages(jsonReturn);
            }
        }

        // Hier worden tweets uit een json file naar zijn juiste klasse weggeschreven en gesynchroniseerd
        // Aangesproken klasse zijn : 'Message', 'Onderwerp', 'Persoon' & 'Hashtag' 
        public void AddMessages(string json)
        {
            initNonExistingRepo();

            ////gebruik deze voor het inladen van een json file 
            ////    (vb: sourceUrl = path naar testdata.json)
            //StreamReader r = new StreamReader(sourceUrl);
            //string json = r.ReadToEnd();
            //List<Message> messages = new List<Message>();

            dynamic tweets = JsonConvert.DeserializeObject(json);

            //initialisatie van velden voor array
            string word1 = null, word2 = null, word3 = null, word4 = null, word5 = null;
            string mention1 = null, mention2 = null, mention3 = null, mention4 = null, mention5 = null;
            string url1 = null, url2 = null;

            // Arrays die we gaan opvullen met arrayobjecten uit de dynamische json objecten
            // Dit zijn parameters waar geen afzonderlijke klasse voorzien is
            string[] words = { word1, word2, word3, word4, word5 };
            string[] mentions = { mention1, mention2, mention3, mention4, mention5 };
            string[] urls = { url1, url2 };

            foreach (var item in tweets) //.records
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

                    Retweet = item.retweet,
                    Date = item.date,

                    Gender = item.profile.gender,
                    Age = item.profile.age,
                    Education = item.profile.education,
                    Language = item.profile.language,
                    Personality = item.profile.personality,

                    Word1 = words[0],
                    Word2 = words[1],
                    Word3 = words[2],
                    Word4 = words[3],
                    Word5 = words[4],

                    Mention1 = mentions[0],
                    Mention2 = mentions[1],
                    Mention3 = mentions[2],
                    Mention4 = mentions[3],
                    Mention5 = mentions[4],

                    Url1 = urls[0],
                    Url2 = urls[1],

                    SubjectMessages = new Collection<SubjectMessage>()
                };

                try
                {
                    tweet.SentimentPos = item.sentiment[0];
                    tweet.SentimentNeg = item.sentiment[1];
                }
                catch { }

                try
                {
                    tweet.Geo1 = item.geo[0];
                    tweet.Geo2 = item.geo[1];
                }
                catch { }

                repo.AddingMessage(tweet);

                foreach (string person in item.persons)
                {
                    Persoon persoon = AddPersoon(person);
                    AddSubjectMessage(tweet, persoon);
                }

                foreach (string hashtag in item.hashtags)
                {
                    Hashtag hasht = AddHashtag(hashtag);
                    AddSubjectMessage(tweet, hasht);
                }
            }
        }

        // We gaan kijken of de 'Persoon' al in de databank bestaat.
        // Zoja: De bestaande 'Persoon' wordt meegegeven
        // Zonee: Een nieuwe 'Persoon' wordt geïnitialiseerd en meegegeven
        public Persoon AddPersoon(string naam)
        {
            initNonExistingRepo();

            Persoon persoon;
            IEnumerable<Persoon> personen = repo.ReadPersonen();

            bool ifExists = personen.Any(x => x.Naam == naam);

            if (ifExists == true)
            {
                persoon = personen.FirstOrDefault(x => x.Naam == naam);
            }
            else
            {
                persoon = new Persoon()
                {
                    Naam = naam,
                    SubjectMessages = new Collection<SubjectMessage>()
                };
                repo.AddOnderwerp(persoon);
            }
            return persoon;
        }

        // We gaan kijken of de 'Hashtag' al in de databank bestaat.
        // Zoja: De bestaande 'Hashtag' wordt meegegeven
        // Zonee: Een nieuwe 'Hashtag' wordt geïnitialiseerd en meegegeven
        public Hashtag AddHashtag(string hashtag)
        {
            initNonExistingRepo();

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

        // Toevoegen van een SubjectMessage adhv een 'Message' en een 'Persoon'
        public void AddSubjectMessage(Message msg, Persoon persoon)
        {
            initNonExistingRepo();

            SubjectMessage subjMess = new SubjectMessage()
            {
                Msg = msg,
                Persoon = persoon
            };
            repo.AddSubjectMsg(subjMess);
        }

        // Toevoegen van een SubjectMessage adhv een 'Message' en een 'Hashtag'
        public void AddSubjectMessage(Message msg, Hashtag hashtag)
        {
            initNonExistingRepo();

            SubjectMessage subjMess = new SubjectMessage()
            {
                Msg = msg,
                Hashtag = hashtag
            };
            repo.AddSubjectMsg(subjMess);
        }

        // We vragen adhv een methode in de repo een lijst in te laden
        // Deze methode wordt aangesproken in de 'GebruikerManager'
        public IEnumerable<Onderwerp> ReadOnderwerpen()
        {
            initNonExistingRepo();

            IEnumerable<Onderwerp> onderwerpen = repo.ReadSubjects();
            return onderwerpen;
        }

        public Organisatie AddOrganisation(string naamOrganisatie)
        {
            initNonExistingRepo();

            Organisatie organisatie;
            IEnumerable<Organisatie> organisaties = repo.ReadOrganisaties();

            bool ifExists = organisaties.Any(x => x.NaamOrganisatie == naamOrganisatie);

            if (ifExists == true)
            {
                organisatie = organisaties.FirstOrDefault(x => x.NaamOrganisatie == naamOrganisatie);
            }
            else
            {
                organisatie = new Organisatie()
                {
                    NaamOrganisatie = naamOrganisatie,
                    Tewerkstellingen = new Collection<Tewerkstelling>()
                };
                repo.AddOnderwerp(organisatie);
            }
            return organisatie;
        }

        public void AddOrganisations(string filePath)
        {
            initNonExistingRepo();

            // Json /CSV
        }

        public void AddTewerkstelling(string naam, string naamOrganisatie)
        {
            initNonExistingRepo();

            Persoon persoon;
            Organisatie organisatie;

            //lijst personen en organisaties opvragen
            IEnumerable<Persoon> personen = repo.ReadPersonen();
            IEnumerable<Organisatie> organisaties = repo.ReadOrganisaties();

            //kijken of persoon en organisatie bestaan
            bool ifExistsP = personen.Any(x => x.Naam == naam);
            bool ifExistsO = organisaties.Any(x => x.NaamOrganisatie == naamOrganisatie);

            //persoon & organisatie initialiseren
            if (ifExistsP)
            {
                persoon = personen.FirstOrDefault(x => x.Naam == naam);
            }
            else
            {
                throw new ArgumentException("Persoon '" + naam + "' not found!");
            }
            if (ifExistsO)
            {
                organisatie = organisaties.FirstOrDefault(x => x.NaamOrganisatie == naamOrganisatie);
            }
            else
            {
                throw new ArgumentException("Organisatie '" + naamOrganisatie + "' not found!");
            }

            //'Tewerkstelling' initialiseren
            Tewerkstelling tewerkstelling = new Tewerkstelling()
            {
                Persoon = persoon,
                Organisatie = organisatie
            };

            //Tewerkstelling toevoegen aan de ICollection van 'Persoon'
            var persoonColl = persoon.Tewerkstellingen;
            if (persoonColl != null)
            {
                persoon.Tewerkstellingen = persoonColl.ToList();
            }
            else
            {
                persoon.Tewerkstellingen = new Collection<Tewerkstelling>();
            }

            persoon.Tewerkstellingen.Add(tewerkstelling);

            //Tewerkstelling toevoegen aan de ICollection van 'Organisatie'
            var organisatieColl = organisatie.Tewerkstellingen;
            if (organisatieColl != null)
            {
                organisatie.Tewerkstellingen = organisatieColl.ToList();
            }
            else
            {
                organisatie.Tewerkstellingen = new Collection<Tewerkstelling>();
            }

            organisatie.Tewerkstellingen.Add(tewerkstelling);

            //eerst tewerkstelling creëren zodat deze een PK toegewegen krijgt
            repo.AddingTewerkstelling(tewerkstelling);
            //dan de persoon & organisatie updaten met de nieuwe 'Tewerkstelling'
            //Todo: misschien gewoon een UpdateContext maken
            repo.UdateOnderwerp(persoon);
        }

        public void AddTewerkstelling(Persoon persoon, string naamOrganisatie)
        {
            initNonExistingRepo();

            //Persoon persoon;
            Organisatie organisatie;

            //lijst personen en organisaties opvragen
            IEnumerable<Persoon> personen = repo.ReadPersonen();
            IEnumerable<Organisatie> organisaties = repo.ReadOrganisaties();

            //kijken of persoon en organisatie bestaan
            bool ifExistsP = personen.Any(x => x.Naam == persoon.Naam);
            bool ifExistsO = organisaties.Any(x => x.NaamOrganisatie == naamOrganisatie);

            //kijken of persoon bestaat 
            if (!ifExistsP)
            {
                throw new ArgumentException("Persoon '" + persoon.Naam + "' not found!");
            }
            //kijken of organisatie bestaat & initialiseren
            if (ifExistsO)
            {
                organisatie = organisaties.FirstOrDefault(x => x.NaamOrganisatie == naamOrganisatie);
            }
            else
            {
                organisatie = AddOrganisation(naamOrganisatie);
            }

            //'Tewerkstelling' initialiseren
            Tewerkstelling tewerkstelling = new Tewerkstelling()
            {
                Persoon = persoon,
                Organisatie = organisatie
            };

            //Tewerkstelling toevoegen aan de ICollection van 'Persoon'
            var persoonColl = persoon.Tewerkstellingen;
            if (persoonColl != null)
            {
                persoon.Tewerkstellingen = persoonColl.ToList();
            }
            else
            {
                persoon.Tewerkstellingen = new Collection<Tewerkstelling>();
            }

            persoon.Tewerkstellingen.Add(tewerkstelling);

            //Tewerkstelling toevoegen aan de ICollection van 'Organisatie'
            var organisatieColl = organisatie.Tewerkstellingen;
            if (organisatieColl != null)
            {
                organisatie.Tewerkstellingen = organisatieColl.ToList();
            }
            else
            {
                organisatie.Tewerkstellingen = new Collection<Tewerkstelling>();
            }

            organisatie.Tewerkstellingen.Add(tewerkstelling);

            //eerst tewerkstelling creëren zodat deze een PK toegewegen krijgt
            repo.AddingTewerkstelling(tewerkstelling);
            //dan de persoon & organisatie updaten met de nieuwe 'Tewerkstelling'
            //Todo: misschien gewoon een UpdateContext maken
            repo.UdateOnderwerp(persoon);
        }

        public void AddPersonen(string pathToJson)
        {
            StreamReader r = new StreamReader(pathToJson);
            string json = r.ReadToEnd();
            List<Message> messages = new List<Message>();

            dynamic persons = JsonConvert.DeserializeObject(json);
            

            foreach(var person in persons)
            {
                initNonExistingRepo();

                Persoon persoon = new Persoon()
                {
                    Naam = person.full_name,
                    District = person.district,
                    Level = person.level,
                    Gender = person.gender,
                    Twitter = person.twitter,
                    Site = person.site,
                    Facebook = person.facebook,
                    Town = person.town,
                    DateOfBirth = person.dateOfBirth,
                //eventueel 'id' integreren, voorlopig niet nodig

                SubjectMessages = new Collection<SubjectMessage>()
                };

                try
                {
                    //deze is soms null
                    persoon.PostalCode = person.postal_code;
                }
                catch { }

                repo.AddOnderwerp(persoon);

                //persoon linken aan een organisatie
                string naamOrganisatie = person.organisation;
                AddTewerkstelling(persoon, naamOrganisatie);
            }
        }

        public int CountSubjMsgsPersoon(Onderwerp onderwerp)
        {
            initNonExistingRepo();

            int countedTweets = 0;

            IEnumerable<Persoon> personen = repo.ReadPersonen();
            IEnumerable<Hashtag> hashtags = repo.ReadHashtags();

            //-- Als je een object meegeeft zet je deze in commentaar /verwijder --//
            //Persoon onderwerp = personen.FirstOrDefault(p => p.OnderwerpId == 256);
            //-----------------------------------------------------------------------//


            IEnumerable<SubjectMessage> subjMsgs = repo.ReadSubjectMessages();

            foreach (var subj in subjMsgs)
            {
                //kijkt of het om een 'persoon' gaat
                if (subj.Persoon != null)
                {
                    if (subj.Persoon.OnderwerpId == onderwerp.OnderwerpId)
                    {
                        countedTweets++;
                    }
                }
                //kijkt of het om een 'hashtag' gaat
                if (subj.Hashtag != null)
                {
                    if (subj.Hashtag.OnderwerpId == onderwerp.OnderwerpId)
                    {
                        countedTweets++;
                    }
                }
            }

            return countedTweets;
        }

        public IEnumerable<Onderwerp> ReadOnderwerpenWithSubjMsgs()
        {
            initNonExistingRepo();
            
            IEnumerable<Onderwerp> onderwerpen = repo.ReadSubjects();
            //-------------deze zijn nodig om automatisch keys te vinden-------------//
            IEnumerable<Persoon> personen = repo.ReadPersonen();
            IEnumerable<Hashtag> hashtags = repo.ReadHashtags();
            IEnumerable<Message> messages = repo.ReadMessages();
            //-----------------------------------------------------------------------//
            IEnumerable<SubjectMessage> subjMsgs = repo.ReadSubjectMessages();

            foreach(var subj in onderwerpen)
            {
                subj.SubjectMessages = new Collection<SubjectMessage>();
            }

            foreach (SubjectMessage subj in subjMsgs)
            {
                try
                {
                    if (subj.Persoon != null)
                    {
                        Persoon prsn = personen.FirstOrDefault(o => o.OnderwerpId == subj.Persoon.OnderwerpId);
                        prsn.SubjectMessages.Add(subj);
                    }
                    else
                    {
                        Onderwerp ondrwrp = onderwerpen.FirstOrDefault(o => o.OnderwerpId == subj.Hashtag.OnderwerpId);
                        ondrwrp.SubjectMessages.Add(subj);
                    }
                }
                catch
                {
                    throw new ArgumentException("SubjectMessage " + subj.SubjectMsgId + " kan niet gelinkt worden");
                }
            }
                return onderwerpen;
        }


        //Unit of Work related
        public void initNonExistingRepo(bool withUnitOfWork = false)
        {
            // Als we een repo met UoW willen gebruiken en als er nog geen uowManager bestaat:
            // Dan maken we de uowManager aan en gebruiken we de context daaruit om de repo aan te maken.
            if (withUnitOfWork)
            {
                if (uowManager == null)
                {
                    uowManager = new UnitOfWorkManager();
                }
                repo = new DAL.MessageRepository(uowManager.UnitOfWork);
            }
            // Als we niet met UoW willen werken, dan maken we een repo aan als die nog niet bestaat.
            else
            {
                //zien of repo al bestaat
                if (repo == null)
                {
                    repo = new DAL.MessageRepository();
                }
                else
                {
                    //checken wat voor repo we hebben
                    bool isUoW = repo.isUnitofWork();
                    if (isUoW)
                    {
                        repo = new DAL.MessageRepository();
                    }
                    else
                    {
                        // repo behoudt zijn context
                    }
                }
            }
        }
    }
}

