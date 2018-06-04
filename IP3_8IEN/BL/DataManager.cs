using IP3_8IEN.BL.Domain.Data;
using Newtonsoft.Json;
using System.IO;

using IP3_8IEN.DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Net;
using System.Web.Script.Serialization;
using System.Net.Mail;
using IP3_8IEN.BL.Domain.Dashboard;
using System.Text;
using System.Globalization;
using IP_8IEN.BL.Domain.Dashboard;

namespace IP3_8IEN.BL
{
    public class DataManager : IDataManager
    {
        private UnitOfWorkManager uowManager;
        private IMessageRepository repo;//= new MessageRepository();
        private IGebruikerManager gebrMgr;
        private IDashManager dashMgr;

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
        public void ApiRequestToJson(bool isReCheck = false)
        {
            {
                string url = "https://kdg.textgain.com/query";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Headers.Add("X-API-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Accept = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";

                string json;

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //query opstellen : named parameters
                    if (isReCheck)
                    {
                        json = new JavaScriptSerializer().Serialize(new
                        {
                            //name = "Annick De Ridder",
                            since = ReadMessagesWithSubjMsgs().ToList().OrderByDescending(m => m.Date).First().Date.ToString("dd MMM yyyy hh:mm", new CultureInfo("en-GB")),
                            //until weglaten --> last scraping
                            // until = "30 Apr 2018 00:01",
                        });
                    }
                    else
                    {
                        json = new JavaScriptSerializer().Serialize(new
                        {
                            //name = "Annick De Ridder",
                            since = "29 Apr 2018 10:31",
                            //until weglaten --> last scraping
                            until = "29 Apr 2018 14:01",
                        });
                    }

                    streamWriter.Write(json);
                }

                JsonSerializer serializer = new JsonSerializer();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    AddMessages(streamReader.ReadToEnd());
                }
            }
        }


        // Hier worden tweets uit een json file naar zijn juiste klasse weggeschreven en gesynchroniseerd
        // Aangesproken klasse zijn : 'Message', 'Onderwerp', 'Persoon' & 'Hashtag' 
        public void AddMessages(string json) //(string sourceUrl) <-- voor json
        {
            InitNonExistingRepo();

            ////gebruik deze voor het inladen van een json file 
            ////    (vb: sourceUrl = path naar testdata.json)
            //StreamReader r = new StreamReader(sourceUrl);
            //string json = r.ReadToEnd();
            //List<Message> messages = new List<Message>();

            dynamic tweets = JsonConvert.DeserializeObject(json);

            //initialisatie van velden voor array
            string word1 = null, word2 = null, word3 = null, word4 = null, word5 = null,
                mention1 = null, mention2 = null, mention3 = null, mention4 = null, mention5 = null,
                url1 = null, url2 = null;

            // Arrays die we gaan opvullen met arrayobjecten uit de dynamische json objecten
            // Dit zijn parameters waar geen afzonderlijke klasse voorzien is
            string[] words = { word1, word2, word3, word4, word5 };
            string[] mentions = { mention1, mention2, mention3, mention4, mention5 };
            string[] urls = { url1, url2 };
            int a = 0;

            foreach (var item in tweets) //.records
            {
                a++;
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
                    tweet.Polarity = item.sentiment[0];
                    tweet.Objectivity = item.sentiment[1];
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
                    tweet.SubjectMessages.Add(AddSubjectMessage(tweet, persoon));
                }

                foreach (string hashtag in item.hashtags)
                {
                    //AddSubjectMessage(tweet, AddHashtag(hashtag));
                    Hashtag hasht = AddHashtag(hashtag);
                    tweet.SubjectMessages.Add(AddSubjectMessage(tweet, hasht));
                }

                repo.UpdateMessage();
            }
        }

        // We gaan kijken of de 'Persoon' al in de databank bestaat.
        // Zoja: De bestaande 'Persoon' wordt meegegeven
        // Zonee: Een nieuwe 'Persoon' wordt geïnitialiseerd en meegegeven
        public Persoon AddPersoon(string naam)
        {
            InitNonExistingRepo();

            Persoon persoon;
            IEnumerable<Persoon> personen = repo.ReadPersonen();

            if (personen.Any(x => x.Naam == naam))
            {
                persoon = personen.FirstOrDefault(x => x.Naam == naam);
            }
            else
            {
                persoon = new Persoon()
                {

                    Naam = naam,
                    // DateTime kan niet null zijn --> voorlopig tijd van creatie meegeven
                    Geboortedatum = DateTime.Now,
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
            InitNonExistingRepo();

            Hashtag hasht;
            IEnumerable<Hashtag> hashtags = repo.ReadHashtags();

            if (hashtags.Any(x => x.Naam == hashtag))
            {
                hasht = hashtags.FirstOrDefault(x => x.Naam == hashtag);
            }
            else
            {
                hasht = new Hashtag()
                {
                    Naam = hashtag,
                    SubjectMessages = new Collection<SubjectMessage>()
                };
                repo.AddOnderwerp(hasht);
            }

            return hasht;
        }

        // Toevoegen van een SubjectMessage adhv een 'Message' en een 'Persoon'
        public SubjectMessage AddSubjectMessage(Message msg, Persoon persoon)
        {
            InitNonExistingRepo();

            SubjectMessage subjMess = new SubjectMessage()
            {
                Msg = msg,
                Persoon = persoon
            };
            repo.AddSubjectMsg(subjMess);

            if(persoon.SubjectMessages == null)
            {
                persoon.SubjectMessages = new Collection<SubjectMessage>
                {
                    subjMess
                };
            } else {
                persoon.SubjectMessages.Add(subjMess);
            }
            
            return subjMess;
        }

        public SubjectMessage AddSubjectMessage(Message msg, Hashtag hashtag)
        {
            InitNonExistingRepo();

            SubjectMessage subjMess = new SubjectMessage()
            {
                Msg = msg,
                Hashtag = hashtag
            };
            repo.AddSubjectMsg(subjMess);

            if (hashtag.SubjectMessages == null)
            {
                hashtag.SubjectMessages = new Collection<SubjectMessage>
                {
                    subjMess
                };
            }
            else
            {
                hashtag.SubjectMessages.Add(subjMess);
            }

            return subjMess;
        }

        // Toevoegen van een SubjectMessage adhv een 'Message' en een 'Hashtag'
        //public void AddSubjectMessage(Message msg, Hashtag hashtag)
        //{
        //    InitNonExistingRepo();
            
        //    repo.AddSubjectMsg(new SubjectMessage()
        //    {
        //        Msg = msg,
        //        Hashtag = hashtag
        //    });
        //}

        // We vragen adhv een methode in de repo een lijst in te laden
        // Deze methode wordt aangesproken in de 'GebruikerManager'
        public IEnumerable<Onderwerp> ReadOnderwerpen()
        {
            InitNonExistingRepo();
            
            return repo.ReadSubjects();
        }

        public Organisatie AddOrganisation(string naamOrganisatie)
        {
            InitNonExistingRepo();

            Organisatie organisatie;
            IEnumerable<Organisatie> organisaties = repo.ReadOrganisaties();

            if (organisaties.Any(x => x.Naam == naamOrganisatie))
            {
                organisatie = organisaties.FirstOrDefault(x => x.Naam == naamOrganisatie);
            }
            else
            {
                organisatie = new Organisatie()
                {
                    Naam = naamOrganisatie,
                    Tewerkstellingen = new Collection<Tewerkstelling>()
                };
                repo.AddOnderwerp(organisatie);
            }
            return organisatie;
        }

        public void AddOrganisations(string filePath) => InitNonExistingRepo();// Json /CSV

        public void AddTewerkstelling(string naam, string naamOrganisatie)
        {
            InitNonExistingRepo();

            Persoon persoon;
            Organisatie organisatie;

            //lijst personen en organisaties opvragen
            IEnumerable<Persoon> personen = repo.ReadPersonen();
            IEnumerable<Organisatie> organisaties = repo.ReadOrganisaties();

            //persoon & organisatie initialiseren indien ze bestaan
            if (personen.Any(x => x.Naam == naam))
            {
                persoon = personen.FirstOrDefault(x => x.Naam == naam);
            }
            else
            {
                throw new ArgumentException("Persoon '" + naam + "' not found!");
            }
            if (organisaties.Any(x => x.Naam == naamOrganisatie))
            {
                organisatie = organisaties.FirstOrDefault(x => x.Naam == naamOrganisatie);
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
            InitNonExistingRepo();

            //Persoon persoon;
            Organisatie organisatie;

            //lijst organisaties opvragen
            IEnumerable<Organisatie> organisaties = repo.ReadOrganisaties();
            
            //kijken of persoon bestaat 
            if (!repo.ReadPersonen().Any(x => x.Naam == persoon.Naam))
            {
                throw new ArgumentException("Persoon '" + persoon.Naam + "' not found!");
            }
            //kijken of organisatie bestaat & initialiseren
            if (organisaties.Any(x => x.Naam == naamOrganisatie))
            {
                organisatie = organisaties.FirstOrDefault(x => x.Naam == naamOrganisatie);
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
            InitNonExistingRepo();
            
            string json = new StreamReader(pathToJson).ReadToEnd();
            List<Message> messages = new List<Message>();

            dynamic persons = JsonConvert.DeserializeObject(json);


            foreach (var person in persons)
            {
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
                    //eventueel 'id' integreren, voorlopig niet nodig

                    SubjectMessages = new Collection<SubjectMessage>()
                };

                try
                {
                    //DateTime kan niet 'null' zijn
                    persoon.Geboortedatum = person.dateOfBirth;
                }
                catch
                {
                    persoon.Geboortedatum = DateTime.Now;
                }

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

        public string ExportToCSV(IEnumerable<Persoon> personen)
        {
            string json = JsonConvert.SerializeObject(personen, Formatting.Indented);
            return json;
        }

        public int CountSubjMsgsPersoon(Onderwerp onderwerp)
        {
            InitNonExistingRepo();

            int countedTweets = 0;

            IEnumerable<Persoon> personen = repo.ReadPersonen();
            IEnumerable<Hashtag> hashtags = repo.ReadHashtags();


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

        public IEnumerable<Message> ReadMessagesWithSubjMsgs()
        {
            InitNonExistingRepo();

            IEnumerable<Message> messages = repo.ReadMessages(true);

            return messages;
        }

        public Persoon GetPersoon(int persoonId)
        {
            InitNonExistingRepo();

            Persoon persoon = repo.ReadPersoon(persoonId);
            return persoon;
        }
        public Persoon GetPersoonWithSjctMsg(int persoonId)
        {
            InitNonExistingRepo();

            Persoon persoon = repo.ReadPersoonWithSbjctMsg(persoonId);
            return persoon;
        }

        public Organisatie GetOrganisatie(int organisatieId)
        {
            InitNonExistingRepo();

            Organisatie organisatie = repo.ReadOrganisatie(organisatieId);
            return organisatie;
        }

        public IEnumerable<Persoon> GetPersonen()
        {
            InitNonExistingRepo();
            IEnumerable<Persoon> personen = repo.ReadPersonen();
            return personen;
        }

        public IEnumerable<Organisatie> GetOrganisaties()
        {
            InitNonExistingRepo();
            IEnumerable<Organisatie> organisaties = repo.ReadOrganisaties();
            return organisaties;
        }

        public void ChangeOrganisation(Organisatie organisatie)
        {
            InitNonExistingRepo();
            repo.EditOrganisation(organisatie);
        }

        public void ChangePersoon(Persoon persoon)
        {
            InitNonExistingRepo();
            repo.EditPersoon(persoon);
        }

        public Persoon GetPersoon(string naam)
        {
            InitNonExistingRepo();
            Persoon persoon = repo.ReadPersoon(naam);

            return persoon;
        }

        public Persoon GetPersoonWithTewerkstelling(string naam)
        {
            InitNonExistingRepo();
            Persoon personen = repo.ReadPersoonWithTewerkstelling(naam);
            return personen;
        }
        public Persoon GetPersoonWithTewerkstelling(int id)
        {
            InitNonExistingRepo();
            Persoon personen = repo.ReadPersoonWithTewerkstelling(id);
            return personen;
        }

        public IEnumerable<Hashtag> GetHashtags()
        {
            InitNonExistingRepo();
            IEnumerable<Hashtag> hashtags = repo.ReadHashtags().ToList();
            return hashtags;
        }

        public void UpdateHashtags(IEnumerable<Hashtag> hashtags)
        {
            InitNonExistingRepo();

            foreach(Hashtag hash in hashtags)
            {
                repo.UpdateHashtag(hash);
            }
        }

        public void CreateTheme(string naam, string beschrijving, IEnumerable<Hashtag> hashForTheme)
        {
            InitNonExistingRepo();
            List<string> hashtags = new List<string>();

            foreach(Hashtag hash in hashForTheme)
            {
                hashtags.Add(hash.Naam);
            }

            Thema theme = new Thema
            {
                Naam = naam,
                Beschrijving = beschrijving
            };
            repo.CreateTheme(theme);

            int count = hashForTheme.Count();

            switch (count)
            {
                case 0:
                    break;
                case 1:
                    theme.Hashtag1 = hashtags[0];
                    break;
                case 2:
                    theme.Hashtag1 = hashtags[0];
                    theme.Hashtag2 = hashtags[1];
                    break;
                case 3:
                    theme.Hashtag1 = hashtags[0];
                    theme.Hashtag2 = hashtags[1];
                    theme.Hashtag3 = hashtags[2];
                    break;
                case 4:
                    theme.Hashtag1 = hashtags[0];
                    theme.Hashtag2 = hashtags[1];
                    theme.Hashtag3 = hashtags[2];
                    theme.Hashtag4 = hashtags[3];
                    break;
                default:
                    break;
            }

            repo.UpdateTheme(theme);
        }

        public IEnumerable<Thema> GetThemas()
        {
            InitNonExistingRepo();
            return repo.ReadThemas();
        }

        public void UpdateThema(Thema thema)
        {
            InitNonExistingRepo();
            repo.UpdateTheme(thema);
        }

        public Thema GetThema(int id)
        {
            InitNonExistingRepo();
            return repo.ReadThemas(id);
        }

        //Unit of Work related
        public void InitNonExistingRepo(bool withUnitOfWork = false)
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
                    bool isUoW = repo.IsUnitofWork();
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



        public class ZScore
        {
            private string politician;
            private double score;

            public ZScore(string politician, double score)
            {
                this.politician = politician;
                this.score = score;
            }
            public override string ToString() => $"Politieker {politician}\n \t score: {score}";
        }

        public void GetAlerts()
        {
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<ZScore> zscores = new List<ZScore>();
            List<String> namen = new List<string>();
            int totaalTweets;
            double gemiddelde;
            DateTime laatsteTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            List<SubjectMessage> subjectmessages = new List<SubjectMessage>();


            foreach (Message m in messages)
            {
                subjectmessages.AddRange(m.SubjectMessages.Where(r => r.Persoon != null).ToList());
            }

            foreach (SubjectMessage s in subjectmessages)
            {
                namen.Add(s.Persoon.Naam);
                namen = namen.Distinct().ToList();
            }


            List<int> tweetsPerDag = new List<int>();
            foreach (string s in namen)
            {
                totaalTweets = 0;
                //totaalTweets = messages.Where(Message => Message.Politician == s).Count();
                bool test;
                List<Message> ms = new List<Message>();

                foreach (Message m in messages)
                {
                    test = false;
                    foreach (SubjectMessage sm in m.SubjectMessages)
                    {
                        if (sm.Persoon != null && sm.Persoon.Naam == s)
                        {
                            test = true;
                        }
                    }
                    if (test)
                    {
                        totaalTweets++;
                        ms.Add(m);
                    }
                }

                //Message mm = messages.Where(Message => Message.Politician == s).OrderBy(o=>o.Date).First();
                DateTime start = messages.OrderBy(m => m.Date).ToList().First().Date;
                tweetsPerDag.Clear();
                do
                {
                    tweetsPerDag.Add(ms.Where(m => m.Date.Date == start.Date).Count());
                    //tweetsPerDag.Add(messages.Where(Message => Message.Politician == s).Where(Message => Message.Date.Date == start).Count());
                    start = start.AddDays(1);
                } while (start <= laatsteTweet);
                double totaal = 0;
                foreach (int i in tweetsPerDag)
                {
                    totaal = totaal + i;
                }
                gemiddelde = totaal / tweetsPerDag.Count();

                double average = tweetsPerDag.Average();
                double sumOfSquaresOfDifferences = tweetsPerDag.Select(val => (val - average) * (val - average)).Sum();
                double sd = Math.Sqrt(sumOfSquaresOfDifferences / tweetsPerDag.Count());
                

                zscores.Add(new ZScore(s, (tweetsPerDag.Last() - gemiddelde) / sd));
            }
            

            GetTweetsPerDagList(repo.ReadPersonen().ToList().Where(p => p.Naam == "Jan Jambon").First());
        }

        public void SendMail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("integratieproject.8ien@gmail.com");
                mail.To.Add("thomas.dewitte@student.kdg.be");
                mail.Subject = "Test";
                mail.Body = "This is for testing SMTP mail from GMAIL";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("integratieproject.8ien@gmail.com", "integratieproject");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Mail was not send" + ex);
            }
        }

        ////public Dictionary<Persoon, double> GetRanking(int aantal, int interval_uren, bool puntNotatie = true)
        //public List<GraphData> GetRanking(int aantal, int interval_uren, bool puntNotatie = false)
        
        //    {
        //    initNonExistingRepo();
        //    List<Persoon> personen = repo.ReadPersonen().ToList();
        //    List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
        //    DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
        //    int laatstePeriode;
        //    int voorlaatstePeriode;
        //        //Sam
        //        //Dictionary<Persoon, double> ranking = new Dictionary<Persoon, double>();
        //        List<GraphData> ranking = new List<GraphData>();
        //        foreach (Persoon p in personen)
        //    {
        //        int teller = messages.Where(m => m.IsFromPersoon(p)).Count();
        //        List<Message> messages2 = messages.Where(m => m.IsFromPersoon(p)).ToList();
        //        laatstePeriode = messages2.Where(m => lastTweet.AddHours(interval_uren * -1) < m.Date).Count();
        //        voorlaatstePeriode = messages2.Where(m => lastTweet.AddHours((interval_uren * 2) * -1) < m.Date && m.Date < lastTweet.AddHours(interval_uren * -1)).Count();
        //        if (puntNotatie == true)
        //        {
        //            ranking.Add(new GraphData(p.Naam, (int)CalculateChange(voorlaatstePeriode, laatstePeriode)));
        //        }
        //        else
        //        {
        //            ranking.Add(new GraphData(p.Naam, (int)CalculateChange(voorlaatstePeriode, laatstePeriode) * 100));
        //        }
        //        /*if (laatstePeriode != 0 && voorlaatstePeriode != 0)
        //        {
        //            ranking.Add(p, ((laatstePeriode - voorlaatstePeriode) / voorlaatstePeriode) * 100);
        //        }*/
        //    }

        //    foreach (var v in ranking)
        //    {
        //        //System.Diagnostics.Debug.WriteLine(v.Key.Naam + " " + v.Value);
        //    }

        //        ranking = ranking.OrderByDescending(r => r.value).ToList();
        //    return ranking.GetRange(0, aantal);
        // }

        public double CalculateChange(long previous, long current)
        {
            if (previous != 0)
            {

                var change = current - previous;
                return (double)change / previous;
            }
            return 0;
        }


        public int GetNumber(Persoon persoon, int laatsteAantalUren = 0)
        {
            InitNonExistingRepo();
            List<Message> messages = repo.ReadMessages(true).ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            int aantal;

            if (laatsteAantalUren == 0)
            {
                aantal = messages.Where(m => m.IsFromPersoon(persoon)).Count();
            }
            else
            {
                aantal = messages.Where(m => m.IsFromPersoon(persoon) && m.Date > lastTweet.AddHours(laatsteAantalUren * -1)).Count();
            }

            return aantal;
        }

        public List<GraphData> GetNumberGraph(Persoon persoon, int laatsteAantalUren = 0)
        {
            InitNonExistingRepo(true);
            dashMgr = new DashManager();
            List<Message> messages = repo.ReadMessages(true).ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            int count = 0;

            List<GraphData> GraphDataList = new List<GraphData>();

            if(persoon.SubjectMessages != null)
            {
                IEnumerable<SubjectMessage> subjMsgs = persoon.SubjectMessages.Where(s => s.Msg.Date > lastTweet.AddHours(laatsteAantalUren * -1)).ToList();
                //////////////////////////////////////////////////////////////////////////
                foreach (SubjectMessage s in subjMsgs)
                {
                    count++;
                }
                GraphData graph = new GraphData(lastTweet.ToString(), count);
                dashMgr.AddGraph(graph);

                GraphDataList.Add(graph);

                lastTweet = lastTweet.AddDays(-1);
                uowManager.Save();
            }

            return GraphDataList;
        }

        public List<GraphData> GetNumberGraph(Organisatie organisatie, int laatsteAantalUren = 0)
        {
            InitNonExistingRepo(true);
            dashMgr = new DashManager();
            List<Message> messages = repo.ReadMessages(true).ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            int count = 0;

            List<GraphData> GraphDataList = new List<GraphData>();

            IEnumerable<Persoon> personen = repo.ReadPersonenWithTewerkstelling();
            //////////////////////////////////////////////////////////////////////////
            foreach (Persoon p in personen)
            {
                foreach (Tewerkstelling t in p.Tewerkstellingen)
                {
                    if (t.Organisatie.Naam == organisatie.Naam)
                    {
                        count += p.SubjectMessages.Where(s => s.Msg.Date > lastTweet.AddHours(laatsteAantalUren * -1)).Count();
                    }
                }
            }
            GraphData graph = new GraphData(lastTweet.ToString(), count);
            dashMgr.AddGraph(graph);

            GraphDataList.Add(graph);

            lastTweet = lastTweet.AddDays(-1);
            uowManager.Save();

            return GraphDataList;
        }

        public List<GraphData> GetNumberGraph(Thema thema, int laatsteAantalUren = 0)
        {
            InitNonExistingRepo(true);
            dashMgr = new DashManager();
            List<Message> messages = repo.ReadMessages(true).ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            int count = 0;

            IEnumerable<Thema> themas = repo.ReadThemas();
            List<Hashtag> hashtags = new List<Hashtag>();
            if(thema.Hashtag1 != null)
            {
                hashtags.Add(hashtags.FirstOrDefault(t => t.Naam == thema.Hashtag1));
            }
            if (thema.Hashtag2 != null)
            {
                hashtags.Add(hashtags.FirstOrDefault(t => t.Naam == thema.Hashtag2));
            }
            if (thema.Hashtag3 != null)
            {
                hashtags.Add(hashtags.FirstOrDefault(t => t.Naam == thema.Hashtag3));
            }
            if (thema.Hashtag4 != null)
            {
                hashtags.Add(hashtags.FirstOrDefault(t => t.Naam == thema.Hashtag4));
            }

            List<GraphData> GraphDataList = new List<GraphData>();

            foreach(Hashtag hash in hashtags)
            {
                count =+ hash.SubjectMessages.Where(s => s.Msg.Date > lastTweet.AddHours(laatsteAantalUren * -1)).Count();
            }

                GraphData graph = new GraphData(lastTweet.ToString(), count);
                dashMgr.AddGraph(graph);

                GraphDataList.Add(graph);

                lastTweet = lastTweet.AddDays(-1);
                uowManager.Save();

            return GraphDataList;
        }

        public List<GraphData2> GetTweetsPerDag2(Persoon persoon1, Persoon persoon2, Persoon persoon3, Persoon persoon4, Persoon persoon5, int aantalDagenTerug = 0)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            DateTime stop = new DateTime();

            if (aantalDagenTerug == 0)
            {
                stop = messages.OrderBy(m => m.Date).ToList().First().Date;
            }
            else
            {
                stop = messages.OrderBy(m => m.Date).ToList().Last().Date;
                stop.AddDays(aantalDagenTerug * -1);
            }

            Dictionary<DateTime, int> tweetsPerDag = new Dictionary<DateTime, int>();
            //Sam
            List<GraphData2> GraphDataList = new List<GraphData2>();
            /*
            do
            {
                //Sam
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                //Sam
                GraphDataList.Add(new GraphData(date, messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFrom(persoon)).Count()));

                tweetsPerDag.Add(lastTweet.Date, messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFrom(persoon)).Count());
                lastTweet = lastTweet.AddDays(-1);
            } while (lastTweet >= stop);*/
            
            for (int i = 0; i < aantalDagenTerug + 1; i++)
            {
                //Sam
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                //Sam
                GraphDataList.Add(new GraphData2(date, messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFromPersoon(persoon1)).Count(), messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFromPersoon(persoon2)).Count(), messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFromPersoon(persoon3)).Count(), messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFromPersoon(persoon4)).Count(), messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFromPersoon(persoon5)).Count()));
                lastTweet = lastTweet.AddDays(-1);
            }
            

            return GraphDataList;
        }

        public string UseApiTwitter(string screenname)
        {
            var oAuthConsumerKey = "dCmrMXgbBJmlac5MWoNy9lrPK";
            var oAuthConsumerSecret = "IiWOQV6SL1KwGMzZY8IgYOH2k9rbPfci3JYwhiNOYjBPWO3cm8";
            var oAuthUrl = "https://api.twitter.com/oauth2/token";


            // Do the Authenticate
            var authHeaderFormat = "Basic {0}";

            var authHeader = string.Format(authHeaderFormat,
                 Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
                        Uri.EscapeDataString((oAuthConsumerSecret)))
                        ));

            var postBody = "grant_type=client_credentials";

            HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(oAuthUrl);
            authRequest.Headers.Add("Authorization", authHeader);
            authRequest.Method = "POST";
            authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (Stream stream = authRequest.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }

            authRequest.Headers.Add("Accept-Encoding", "gzip");

            WebResponse authResponse = authRequest.GetResponse();
            //deserialize into an object
            TwitAuthenticateResponse twitAuthResponse;
            using (authResponse)
            {
                using (var reader = new StreamReader(authResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objectText = reader.ReadToEnd();
                    twitAuthResponse = JsonConvert.DeserializeObject<TwitAuthenticateResponse>(objectText);
                }
            }

            // Do the avatar
            var avatarFormat =
                "https://api.twitter.com/1.1/users/show.json?screen_name={0}";
            var avatarUrl = string.Format(avatarFormat, screenname);
            HttpWebRequest avatarRequest = (HttpWebRequest)WebRequest.Create(avatarUrl);
            var timelineHeaderFormat = "{0} {1}";
            avatarRequest.Headers.Add("Authorization",
                                        string.Format(timelineHeaderFormat, twitAuthResponse.token_type,
                                                      twitAuthResponse.access_token));
            avatarRequest.Method = "Get";
            WebResponse timeLineResponse = avatarRequest.GetResponse();

            var avatarJson = string.Empty;
            using (authResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    avatarJson = reader.ReadToEnd();
                }
            }
            return avatarJson;
        }

        public string GetImageString(string screenname)
        {
            string imageBig = "../Fotos/unknownAvatar.png";
            if (screenname != "" && screenname != null)
            {
                string avatarJson = UseApiTwitter(screenname);
                dynamic items = JsonConvert.DeserializeObject(avatarJson);
                string image = items.profile_image_url_https;
                imageBig = image.Replace("_normal", "");
            }
            //System.Diagnostics.Debug.WriteLine("de avatar string: " + imageBig);
            return imageBig;
        }
        public string GetBannerString(string screenname)
        {
            string banner = "../Fotos/banner.png";
            if (screenname != "" && screenname != null)
            {
                string avatarJson = UseApiTwitter(screenname);
                dynamic items = JsonConvert.DeserializeObject(avatarJson);
                banner = items.profile_banner_url;
            }
            //System.Diagnostics.Debug.WriteLine("de banner string: " + banner);
            return banner;
        }



        //VIC

        public double GetPolarityByPerson(Persoon persoon)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            double totalPolarity = 0;

            messages = messages.Where(m => m.IsFromPersoon(persoon)).ToList();

            foreach (Message m in messages)
            {
                totalPolarity += m.Polarity;
            }

            return totalPolarity / messages.Count;
        }

        public double GetPolarityByPerson(Persoon persoon, DateTime start)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            double totalPolarity = 0;

            messages = messages.Where(m => m.IsFromPersoon(persoon) && m.Date > start).ToList();

            foreach (Message m in messages)
            {
                totalPolarity += m.Polarity;
            }

            return totalPolarity / messages.Count;
        }

        public double GetPolarityByPerson(Persoon persoon, DateTime start, DateTime stop)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            double totalPolarity = 0;

            messages = messages.Where(m => m.IsFromPersoon(persoon) && m.Date > start && m.Date < stop).ToList();

            foreach (Message m in messages)
            {
                totalPolarity += m.Polarity;
            }

            return totalPolarity / messages.Count;
        }

        public double GetObjectivityByPerson(Persoon persoon)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            double totalObjectivity = 0;

            messages = messages.Where(m => m.IsFromPersoon(persoon)).ToList();

            foreach (Message m in messages)
            {
                totalObjectivity += m.Objectivity;
            }

            return totalObjectivity / messages.Count;
        }

        public double GetObjectivityByPerson(Persoon persoon, DateTime start)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            double totalObjectivity = 0;

            messages = messages.Where(m => m.IsFromPersoon(persoon) && m.Date > start).ToList();

            foreach (Message m in messages)
            {
                totalObjectivity += m.Objectivity;
            }

            return totalObjectivity / messages.Count;
        }

        public double GetObjectivityByPerson(Persoon persoon, DateTime start, DateTime stop)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            double totalObjectivity = 0;

            messages = messages.Where(m => m.IsFromPersoon(persoon) && m.Date > start && m.Date < stop).ToList();

            foreach (Message m in messages)
            {
                totalObjectivity += m.Objectivity;
            }

            return totalObjectivity / messages.Count;
        }

        public int GetMentionCountByName(string naam)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            int counter = 0;

            foreach (Message m in messages)
            {
                if (m.Mention1 == naam || m.Mention2 == naam || m.Mention3 == naam || m.Mention4 == naam || m.Mention5 == naam)
                {
                    counter++;
                }
            }
            return counter;
        }

        public int GetMentionCountByName(string naam, DateTime start)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            int counter = 0;
            messages = messages.Where(m => m.Date > start).ToList();

            foreach (Message m in messages)
            {
                if (m.Mention1 == naam || m.Mention2 == naam || m.Mention3 == naam || m.Mention4 == naam || m.Mention5 == naam)
                {
                    counter++;
                }
            }
            return counter;
        }

        public int GetMentionCountByName(string naam, DateTime start, DateTime stop)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            int counter = 0;

            messages = messages.Where(m => m.Date > start && m.Date < stop).ToList();

            foreach (Message m in messages)
            {
                if (m.Mention1 == naam || m.Mention2 == naam || m.Mention3 == naam || m.Mention4 == naam || m.Mention5 == naam)
                {
                    counter++;
                }
            }
            return counter;
        }

        public List<GraphData> GetTopWordsCount()
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> words = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;

            foreach (Message m in messages)
            {
                words.Add(m.Word1);
                words.Add(m.Word2);
                words.Add(m.Word3);
                words.Add(m.Word4);
                words.Add(m.Word5);
            }

            words = words.Distinct().ToList();

            foreach (String w in words)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Word1 == w || m.Word2 == w || m.Word3 == w || m.Word4 == w || m.Word5 == w)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(w, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data;
        }

        public List<GraphData> GetTopWordsCount(int aantal)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> words = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;

            foreach (Message m in messages)
            {
                words.Add(m.Word1);
                words.Add(m.Word2);
                words.Add(m.Word3);
                words.Add(m.Word4);
                words.Add(m.Word5);
            }

            words = words.Distinct().ToList();

            foreach (String w in words)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Word1 == w || m.Word2 == w || m.Word3 == w || m.Word4 == w || m.Word5 == w)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(w, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data.GetRange(0, aantal);
        }

        public List<GraphData> GetTopWordsCount(int aantal, DateTime start)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> words = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;

            messages = messages.Where(m => m.Date > start).ToList();

            foreach (Message m in messages)
            {
                words.Add(m.Word1);
                words.Add(m.Word2);
                words.Add(m.Word3);
                words.Add(m.Word4);
                words.Add(m.Word5);
            }

            words = words.Distinct().ToList();

            foreach (String w in words)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Word1 == w || m.Word2 == w || m.Word3 == w || m.Word4 == w || m.Word5 == w)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(w, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data.GetRange(0, aantal);
        }

        public List<GraphData> GetTopWordsCount(int aantal, DateTime start, DateTime stop)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> words = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;

            messages = messages.Where(m => m.Date > start && m.Date < stop).ToList();

            foreach (Message m in messages)
            {
                words.Add(m.Word1);
                words.Add(m.Word2);
                words.Add(m.Word3);
                words.Add(m.Word4);
                words.Add(m.Word5);
            }

            words = words.Distinct().ToList();

            foreach (String w in words)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Word1 == w || m.Word2 == w || m.Word3 == w || m.Word4 == w || m.Word5 == w)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(w, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data.GetRange(0, aantal);
        }

        public List<GraphData> GetTopWordsCount(DateTime start, DateTime stop)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> words = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;

            messages = messages.Where(m => m.Date > start && m.Date < stop).ToList();

            foreach (Message m in messages)
            {
                words.Add(m.Word1);
                words.Add(m.Word2);
                words.Add(m.Word3);
                words.Add(m.Word4);
                words.Add(m.Word5);
            }

            words = words.Distinct().ToList();

            foreach (String w in words)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Word1 == w || m.Word2 == w || m.Word3 == w || m.Word4 == w || m.Word5 == w)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(w, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data;
        }

        public int GetWordCountByName(string name)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            int counter = 0;

            foreach (Message m in messages)
            {
                if (m.Word1 == name || m.Word2 == name || m.Word3 == name || m.Word4 == name || m.Word5 == name)
                {
                    counter++;
                }
            }
            return counter;
        }

        public int GetWordCountByName(string name, DateTime start)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            int counter = 0;

            messages = messages.Where(m => m.Date > start).ToList();

            foreach (Message m in messages)
            {
                if (m.Word1 == name || m.Word2 == name || m.Word3 == name || m.Word4 == name || m.Word5 == name)
                {
                    counter++;
                }
            }
            return counter;
        }

        public int GetWordCountByName(string name, DateTime start, DateTime stop)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            int counter = 0;

            messages = messages.Where(m => m.Date > start && m.Date < stop).ToList();

            foreach (Message m in messages)
            {
                if (m.Word1 == name || m.Word2 == name || m.Word3 == name || m.Word4 == name || m.Word5 == name)
                {
                    counter++;
                }
            }
            return counter;
        }

        //Sam

        public List<DataChart> GetTweetsPerDagDataChart(Persoon persoon, int aantalDagenTerug = 0)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            DateTime stop = new DateTime();

            if (aantalDagenTerug == 0)
            {
                stop = messages.OrderBy(m => m.Date).ToList().First().Date;
            }
            else
            {
                stop = messages.OrderBy(m => m.Date).ToList().Last().Date;
                stop.AddDays(aantalDagenTerug * -1);
            }
            List<DataChart> GraphDataList = new List<DataChart>();

            for (int i = 0; i < aantalDagenTerug + 1; i++)
            {
                //Sam
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                //Sam
                GraphDataList.Add(new DataChart(date, messages.Where(m => m.Date.Date.Day == lastTweet.Date.Day && m.IsFromPersoon(persoon)).Count()));
                lastTweet = lastTweet.AddDays(-1);
            }

            return GraphDataList;
        }

        public List<GraphData> GetTweetsPerDagList(Persoon persoon, int aantalDagenTerug = 0)
        {
            InitNonExistingRepo();

            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            DateTime stop = new DateTime();

            if (aantalDagenTerug == 0)
            {
                stop = messages.OrderBy(m => m.Date).ToList().First().Date;
            }
            else
            {
                stop = messages.OrderBy(m => m.Date).ToList().Last().Date;
                stop.AddDays(aantalDagenTerug * -1);
            }


            List<GraphData> GraphDataList = new List<GraphData>();
            for (int i = 0; i < aantalDagenTerug + 1; i++)
            {
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                int count = 0;
                if (persoon.SubjectMessages != null)
                {
                    IEnumerable<SubjectMessage> subjMsgs = persoon.SubjectMessages.Where(s => s.Msg.Date.Day == lastTweet.Date.Day).ToList();
                    //////////////////////////////////////////////////////////////////////////
                    foreach (SubjectMessage s in subjMsgs)
                    {
                        count++;
                    }
                    GraphData graph = new GraphData(date, count);
                    GraphDataList.Add(graph);

                    lastTweet = lastTweet.AddDays(-1);
                }
            }

                return GraphDataList;
        }

        public List<GraphData> GetTweetsPerDagList(Organisatie organisatie, int aantalDagenTerug = 0)
        {
            InitNonExistingRepo();

            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            DateTime stop = new DateTime();

            if (aantalDagenTerug == 0)
            {
                stop = messages.OrderBy(m => m.Date).ToList().First().Date;
            }
            else
            {
                stop = messages.OrderBy(m => m.Date).ToList().Last().Date;
                stop.AddDays(aantalDagenTerug * -1);
            }


            List<GraphData> GraphDataList = new List<GraphData>();
            for (int i = 0; i < aantalDagenTerug + 1; i++)
            {
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                int count = 0;
                IEnumerable<Persoon> personen = repo.ReadPersonenWithTewerkstelling();
                //////////////////////////////////////////////////////////////////////////
                foreach (Persoon p in personen)
                {
                    foreach (Tewerkstelling t in p.Tewerkstellingen)
                    {
                        if (t.Organisatie.Naam == organisatie.Naam)
                        {
                            count += p.SubjectMessages.Where(s => s.Msg.Date.Day == lastTweet.Date.Day).Count();
                        }
                    }
                }
                GraphData graph = new GraphData(date, count);
                GraphDataList.Add(graph);
            }
            return GraphDataList;
        }

        public List<GraphData> GetTweetsPerDagList(Thema thema, int aantalDagenTerug = 0)
        {
            InitNonExistingRepo();

            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            DateTime stop = new DateTime();

            IEnumerable<Thema> themas = repo.ReadThemas();
            IEnumerable<Hashtag> hashtagsDb = repo.ReadHashtagsWithSubjMsgs().ToList();
            List<Hashtag> hashtags = new List<Hashtag>();
            if (thema.Hashtag1 != null)
            {
                hashtags.Add(hashtagsDb.FirstOrDefault(t => t.Naam == thema.Hashtag1));
            }
            if (thema.Hashtag2 != null)
            {
                hashtags.Add(hashtagsDb.FirstOrDefault(t => t.Naam == thema.Hashtag2));
            }
            if (thema.Hashtag3 != null)
            {
                hashtags.Add(hashtagsDb.FirstOrDefault(t => t.Naam == thema.Hashtag3));
            }
            if (thema.Hashtag4 != null)
            {
                hashtags.Add(hashtagsDb.FirstOrDefault(t => t.Naam == thema.Hashtag4));
            }

            if (aantalDagenTerug == 0)
            {
                stop = messages.OrderBy(m => m.Date).ToList().First().Date;
            }
            else
            {
                stop = messages.OrderBy(m => m.Date).ToList().Last().Date;
                stop.AddDays(aantalDagenTerug * -1);
            }

            List<GraphData> GraphDataList = new List<GraphData>();
            for (int i = 0; i < aantalDagenTerug + 1; i++)
            {
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                int count = 0;
                //////////////////////////////////////////////////////////////////////////

                foreach (Hashtag hash in hashtags)
                {
                    count += hash.SubjectMessages.Where(s => s.Msg.Date.Day == lastTweet.Date.Day).Count();
                }
                GraphData graph = new GraphData(date, count);
                GraphDataList.Add(graph);

                lastTweet = lastTweet.AddDays(-1);
            }
            return GraphDataList;
        }

        public List<GraphData> GetTweetsPerDag(Persoon persoon, int aantalDagenTerug = 0)
        {
            InitNonExistingRepo(true);

            dashMgr = new DashManager();

            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            DateTime stop = new DateTime();

            if (aantalDagenTerug == 0)
            {
                stop = messages.OrderBy(m => m.Date).ToList().First().Date;
            }
            else
            {
                stop = messages.OrderBy(m => m.Date).ToList().Last().Date;
                stop.AddDays(aantalDagenTerug * -1);
            }

            
            List<GraphData> GraphDataList = new List<GraphData>();
            for (int i = 0; i < aantalDagenTerug + 1; i++)
            {
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                int count = 0;
                if (persoon.SubjectMessages != null)
                {
                    IEnumerable<SubjectMessage> subjMsgs = persoon.SubjectMessages.Where(s => s.Msg.Date.Day == lastTweet.Date.Day).ToList();
                    //////////////////////////////////////////////////////////////////////////
                    foreach (SubjectMessage s in subjMsgs)
                    {
                        count++;
                    }
                    GraphData graph = new GraphData(date, count);
                    dashMgr.AddGraph(graph);

                    GraphDataList.Add(graph);

                    lastTweet = lastTweet.AddDays(-1);
                }
                uowManager.Save();
            }
            return GraphDataList;
        }

        public List<GraphData> GetTweetsPerDag(Organisatie organisatie, int aantalDagenTerug = 0)
        {
            InitNonExistingRepo(true);

            dashMgr = new DashManager();

            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            DateTime stop = new DateTime();

            if (aantalDagenTerug == 0)
            {
                stop = messages.OrderBy(m => m.Date).ToList().First().Date;
            }
            else
            {
                stop = messages.OrderBy(m => m.Date).ToList().Last().Date;
                stop.AddDays(aantalDagenTerug * -1);
            }


            List<GraphData> GraphDataList = new List<GraphData>();
            for (int i = 0; i < aantalDagenTerug + 1; i++)
            {
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                int count = 0;
                    IEnumerable<Persoon> personen = repo.ReadPersonenWithTewerkstelling();
                    //IEnumerable<SubjectMessage> subjMsgs = organisatie.SubjectMessages.Where(s => s.Msg.Date.Day == lastTweet.Date.Day).ToList();
                    //////////////////////////////////////////////////////////////////////////
                    foreach(Persoon p in personen)
                    {
                        foreach (Tewerkstelling t in p.Tewerkstellingen)
                        {
                            if(t.Organisatie.Naam == organisatie.Naam)
                            {
                                count += p.SubjectMessages.Where(s => s.Msg.Date.Day == lastTweet.Date.Day).Count();
                            }
                        }
                    }
                    GraphData graph = new GraphData(date, count);
                    dashMgr.AddGraph(graph);

                    GraphDataList.Add(graph);

                    lastTweet = lastTweet.AddDays(-1);
                uowManager.Save();
            }
            return GraphDataList;
        }

        public List<GraphData> GetTweetsPerDag(Thema thema, int aantalDagenTerug = 0)
        {
            InitNonExistingRepo(true);

            dashMgr = new DashManager();

            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            DateTime stop = new DateTime();

            IEnumerable<Thema> themas = repo.ReadThemas();
            IEnumerable<Hashtag> hashtagsDb = repo.ReadHashtagsWithSubjMsgs().ToList();
            List<Hashtag> hashtags = new List<Hashtag>();
            if (thema.Hashtag1 != null)
            {
                hashtags.Add(hashtagsDb.FirstOrDefault(t => t.Naam == thema.Hashtag1));
            }
            if (thema.Hashtag2 != null)
            {
                hashtags.Add(hashtagsDb.FirstOrDefault(t => t.Naam == thema.Hashtag2));
            }
            if (thema.Hashtag3 != null)
            {
                hashtags.Add(hashtagsDb.FirstOrDefault(t => t.Naam == thema.Hashtag3));
            }
            if (thema.Hashtag4 != null)
            {
                hashtags.Add(hashtagsDb.FirstOrDefault(t => t.Naam == thema.Hashtag4));
            }

            if (aantalDagenTerug == 0)
            {
                stop = messages.OrderBy(m => m.Date).ToList().First().Date;
            }
            else
            {
                stop = messages.OrderBy(m => m.Date).ToList().Last().Date;
                stop.AddDays(aantalDagenTerug * -1);
            }

            List<GraphData> GraphDataList = new List<GraphData>();
            for (int i = 0; i < aantalDagenTerug + 1; i++)
            {
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                int count = 0;
                //////////////////////////////////////////////////////////////////////////

                foreach (Hashtag hash in hashtags)
                {
                    count += hash.SubjectMessages.Where(s => s.Msg.Date.Day == lastTweet.Date.Day).Count();
                }
                GraphData graph = new GraphData(date, count);
                dashMgr.AddGraph(graph);

                GraphDataList.Add(graph);

                lastTweet = lastTweet.AddDays(-1);
                uowManager.Save();
            }
            return GraphDataList;
        }

        public bool IsFromPersoon(Persoon persoon, IEnumerable<SubjectMessage> subjMsgs)
        {
            foreach (SubjectMessage s in subjMsgs)
            {
                if (s.Persoon == persoon)
                {
                    return true;
                }
            }
            return false;
        }

        public List<GraphData> GetTweetsPerDag2(Persoon persoon, int aantalDagenTerug = 0)
        {
            InitNonExistingRepo(true);

            dashMgr = new DashManager();

            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            DateTime stop = new DateTime();

            if (aantalDagenTerug == 0)
            {
                stop = messages.OrderBy(m => m.Date).ToList().First().Date;
            }
            else
            {
                stop = messages.OrderBy(m => m.Date).ToList().Last().Date;
                stop.AddDays(aantalDagenTerug * -1);
            }

            List<GraphData> GraphDataList = new List<GraphData>();
            for (int i = 0; i < aantalDagenTerug + 1; i++)
            {
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;

                
                int count = messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFromPersoon(persoon)).Count();

                GraphData graph = new GraphData(date, messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFromPersoon(persoon)).Count());
                dashMgr.AddGraph(graph);

                GraphDataList.Add(graph);
                
                lastTweet = lastTweet.AddDays(-1);
            }
            uowManager.Save();
            return GraphDataList;
        }

        public List<GraphData> GetRankingList(int aantal, int interval_uren, bool puntNotatie = false)
        {
            InitNonExistingRepo();

            List<Persoon> personen = repo.ReadPersonen().ToList();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData> graphsList = new List<GraphData>();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            int laatstePeriode;
            int voorlaatstePeriode;

            List<GraphData> ranking = new List<GraphData>();
            foreach (Persoon p in personen)
            {
                int teller = messages.Where(m => m.IsFromPersoon(p)).Count();
                List<Message> messages2 = messages.Where(m => m.IsFromPersoon(p)).ToList();
                laatstePeriode = messages2.Where(m => lastTweet.AddHours(interval_uren * -1) < m.Date).Count();
                voorlaatstePeriode = messages2.Where(m => lastTweet.AddHours((interval_uren * 2) * -1) < m.Date && m.Date < lastTweet.AddHours(interval_uren * -1)).Count();
                if (puntNotatie == true)
                {
                    GraphData graph = new GraphData(p.Naam, (double)CalculateChange(voorlaatstePeriode, laatstePeriode));
                    ranking.Add(graph);
                }
                else
                {
                    GraphData graph = new GraphData(p.Naam, CalculateChange(voorlaatstePeriode, laatstePeriode));
                    ranking.Add(graph);
                }
            }
            ranking = ranking.OrderByDescending(r => r.Value).ToList();
            ranking = ranking.GetRange(0, aantal);

            foreach (GraphData graph in ranking)
            {
                graphsList.Add(graph);
            }

            return graphsList;
        }

        public List<GraphData> GetRanking(int aantal, int interval_uren, bool puntNotatie = false)
        {
            InitNonExistingRepo(true);
            dashMgr = new DashManager();

            List<Persoon> personen = repo.ReadPersonen().ToList();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData> graphsList = new List<GraphData>();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            int laatstePeriode;
            int voorlaatstePeriode;
            
            List<GraphData> ranking = new List<GraphData>();
            foreach (Persoon p in personen)
            {
                int teller = messages.Where(m => m.IsFromPersoon(p)).Count();
                List<Message> messages2 = messages.Where(m => m.IsFromPersoon(p)).ToList();
                laatstePeriode = messages2.Where(m => lastTweet.AddHours(interval_uren * -1) < m.Date).Count();
                voorlaatstePeriode = messages2.Where(m => lastTweet.AddHours((interval_uren * 2) * -1) < m.Date && m.Date < lastTweet.AddHours(interval_uren * -1)).Count();
                if (puntNotatie == true)
                {
                    GraphData graph = new GraphData(p.Naam, (double)CalculateChange(voorlaatstePeriode, laatstePeriode));
                    ranking.Add(graph);
                }
                else
                {
                    GraphData graph = new GraphData(p.Naam, CalculateChange(voorlaatstePeriode, laatstePeriode));
                    ranking.Add(graph);
                }
            }
            ranking = ranking.OrderByDescending(r => r.Value).ToList();
            ranking = ranking.GetRange(0, aantal);

            foreach(GraphData graph in ranking)
            {
                dashMgr.AddGraph(graph);
                graphsList.Add(graph);
            }
            uowManager.Save();

            return graphsList;
        }

        public List<int> ExtractListPersoonId(IEnumerable<GraphData> graphDataList)
        {
            IEnumerable<Persoon> personen = repo.ReadPersonen();
            List<int> ListPersoonId = new List<int>();
            foreach(GraphData graph in graphDataList)
            {
                ListPersoonId.Add(personen.FirstOrDefault(p => p.Naam == graph.Label).OnderwerpId);
            }
            return ListPersoonId;
        }

        public List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData> data = new List<GraphData>
            {
                new GraphData(p1.Naam, messages.Where(m => m.IsFromPersoon(p1)).Count()),
                new GraphData(p2.Naam, messages.Where(m => m.IsFromPersoon(p2)).Count())
            };

            return data;
        }

        public List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2, Persoon p3)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData> data = new List<GraphData>
            {
                new GraphData(p1.Naam, messages.Where(m => m.IsFromPersoon(p1)).Count()),
                new GraphData(p2.Naam, messages.Where(m => m.IsFromPersoon(p2)).Count()),
                new GraphData(p3.Naam, messages.Where(m => m.IsFromPersoon(p3)).Count())
            };

            return data;
        }

        public List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2, Persoon p3, Persoon p4)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData> data = new List<GraphData>
            {
                new GraphData(p1.Naam, messages.Where(m => m.IsFromPersoon(p1)).Count()),
                new GraphData(p2.Naam, messages.Where(m => m.IsFromPersoon(p2)).Count()),
                new GraphData(p3.Naam, messages.Where(m => m.IsFromPersoon(p3)).Count()),
                new GraphData(p4.Naam, messages.Where(m => m.IsFromPersoon(p4)).Count())
            };

            return data;
        }

        public List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2, Persoon p3, Persoon p4, Persoon p5)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData> data = new List<GraphData>
            {
                new GraphData(p1.Naam, messages.Where(m => m.IsFromPersoon(p1)).Count()),
                new GraphData(p2.Naam, messages.Where(m => m.IsFromPersoon(p2)).Count()),
                new GraphData(p3.Naam, messages.Where(m => m.IsFromPersoon(p3)).Count()),
                new GraphData(p4.Naam, messages.Where(m => m.IsFromPersoon(p4)).Count()),
                new GraphData(p5.Naam, messages.Where(m => m.IsFromPersoon(p5)).Count())
            };

            return data;
        }

        public List<GraphData> GetTopStoryCount()
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> stories = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;



            foreach (Message m in messages)
            {
                stories.Add(m.Url1);
                stories.Add(m.Url2);
            }

            stories = stories.Distinct().ToList();

            foreach (String s in stories)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Url1 == s || m.Url2 == s)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(s, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data;
        }

        public List<GraphData> GetTopStoryCount(int aantal)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> stories = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;



            foreach (Message m in messages)
            {
                stories.Add(m.Url1);
                stories.Add(m.Url2);
            }

            stories = stories.Distinct().ToList();

            foreach (String s in stories)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Url1 == s || m.Url2 == s)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(s, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data.GetRange(0, aantal);
        }

        public List<GraphData> GetTopStoryCount(int aantal, DateTime start)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> stories = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;

            messages = messages.Where(m => m.Date > start).ToList();

            foreach (Message m in messages)
            {
                stories.Add(m.Url1);
                stories.Add(m.Url2);
            }

            stories = stories.Distinct().ToList();

            foreach (String s in stories)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Url1 == s || m.Url2 == s)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(s, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data.GetRange(0, aantal);
        }

        public List<GraphData> GetTopStoryCount(int aantal, DateTime start, DateTime stop)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> stories = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;

            messages = messages.Where(m => m.Date > start && m.Date < stop).ToList();

            foreach (Message m in messages)
            {
                stories.Add(m.Url1);
                stories.Add(m.Url2);
            }

            stories = stories.Distinct().ToList();

            foreach (String s in stories)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Url1 == s || m.Url2 == s)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(s, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data.GetRange(0, aantal);
        }

        public List<GraphData> GetTopStoryCount(DateTime start, DateTime stop)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> stories = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;

            messages = messages.Where(m => m.Date > start && m.Date < stop).ToList();

            foreach (Message m in messages)
            {
                stories.Add(m.Url1);
                stories.Add(m.Url2);
            }

            stories = stories.Distinct().ToList();

            foreach (String s in stories)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Url1 == s || m.Url2 == s)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(s, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data;
        }

        public List<GraphData> GetTopStoryByPolitician(Persoon persoon)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> stories = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;

            messages = messages.Where(m => m.IsFromPersoon(persoon)).ToList();

            foreach (Message m in messages)
            {
                stories.Add(m.Url1);
                stories.Add(m.Url2);
                stories.Add(m.Word3);
                stories.Add(m.Word4);
                stories.Add(m.Word5);
            }

            stories = stories.Distinct().ToList();

            foreach (String s in stories)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Url1 == s || m.Url2 == s)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(s, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data;

        }
        public List<GraphData2> GetComparisonPersonNumberOfTweetsOverTime(Persoon p1, Persoon p2, Persoon p3, Persoon p4, Persoon p5)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData2> data = new List<GraphData2>();

            DateTime first = messages.OrderBy(m => m.Date).First().Date;

            do
            {
                data.Add(new GraphData2(
                first.ToString(),
                messages.Where(m => m.IsFromPersoon(p1) && m.Date.Date == first.Date).Count(),
                messages.Where(m => m.IsFromPersoon(p2) && m.Date.Date == first.Date).Count(),
                messages.Where(m => m.IsFromPersoon(p3) && m.Date.Date == first.Date).Count(),
                messages.Where(m => m.IsFromPersoon(p4) && m.Date.Date == first.Date).Count(),
                messages.Where(m => m.IsFromPersoon(p5) && m.Date.Date == first.Date).Count()));

                first.AddDays(1);
            } while (first.Date < DateTime.Now.Date);

            return data;
        }

        public List<GraphData> GetTopMentions(int aantal)
        {
            InitNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<string> mentions = new List<string>();
            List<GraphData> data = new List<GraphData>();
            int counter;



            foreach (Message m in messages)
            {
                mentions.Add(m.Mention1);
                mentions.Add(m.Mention2);
                mentions.Add(m.Mention3);
                mentions.Add(m.Mention4);
                mentions.Add(m.Mention5);
            }

            mentions = mentions.Distinct().ToList();

            foreach (String me in mentions)
            {
                counter = 0;

                foreach (Message m in messages)
                {
                    if (m.Mention1 == me || m.Mention2 == me || m.Mention3 == me || m.Mention4 == me || m.Mention5 == me)
                    {
                        counter++;
                    }
                    data.Add(new GraphData(me, counter));
                }
            }

            data = data.OrderByDescending(d => d.Value).ToList();
            return data.GetRange(0, aantal);
        }
        
        public List<GraphData> FrequenteWoorden(ICollection<SubjectMessage> subjMsgs, int ammount)
        {
            Dictionary<string, int> woorden = new Dictionary<string, int>();
            List<string> woordStrings = new List<string>();
            foreach (SubjectMessage subjMsg in subjMsgs)
            {
                #region add all words to temporary list
                foreach (string woord in GetMessageWords(subjMsg.Msg))
                {
                    woordStrings.Add(woord);
                }
                #endregion
            }
            foreach (string woord in woordStrings)
            {
                if (!woorden.ContainsKey(woord))
                {
                    woorden.Add(woord, 1);
                }
                else
                {
                    int value;
                    woorden.TryGetValue(woord, out value);
                    woorden.Remove(woord);
                    woorden.Add(woord, ++value);
                }
            }
            ////woorden.OrderByDescending(r => r.Value).ToList();
            //woorden.ToList().Sort(delegate (KeyValuePair<string, int> kvp1, KeyValuePair<string, int> kvp2)
            //{
            //    return kvp1.Value.CompareTo(kvp2.Value);
            //});
            List<GraphData> topwoorden = new List<GraphData>();
            foreach (KeyValuePair<string, int> kvp in woorden.OrderByDescending(r => r.Value).ToList().Take(ammount))
            {
                topwoorden.Add(new GraphData(kvp.Key, kvp.Value));
            }
            return topwoorden;
        }

        public IEnumerable<string> GetMessageWords(Message msg)
        {
            List<string> words = new List<string>();
            if (msg.Word1 != null && !msg.Word1.Equals(""))
            {
                words.Add(msg.Word1);
            }
            if (msg.Word2 != null && !msg.Word2.Equals(""))
            {
                words.Add(msg.Word2);
            }
            if (msg.Word3 != null && !msg.Word3.Equals(""))
            {
                words.Add(msg.Word3);
            }
            if (msg.Word4 != null && !msg.Word4.Equals(""))
            {
                words.Add(msg.Word4);
            }
            if (msg.Word5 != null && !msg.Word5.Equals(""))
            {
                words.Add(msg.Word5);
            }
            return words;
        }

        public IEnumerable<Persoon> GetPersonenOnly()
        {
            InitNonExistingRepo();
            IEnumerable<Persoon> personen = repo.ReadPersonenOnly();
            return personen;
        }
    }
}
