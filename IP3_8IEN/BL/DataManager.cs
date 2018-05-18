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
using System.Net.Mail;
using IP3_8IEN.BL.Domain.Dashboard;
using IP_8IEN.BL.Domain.Gebruikers;

namespace IP_8IEN.BL
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
        public void ApiRequestToJson()
        {
            {
                string url = "https://kdg.textgain.com/query";

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
                        since = "29 Apr 2018 00:01",
                        //until weglaten --> last scraping
                        until = "30 Apr 2018 00:01",
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
        public void AddMessages(string json) //(string sourceUrl) <-- voor json
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
                    tweet.SubjectMessages.Add(AddSubjectMessage(tweet, persoon));
                }

                foreach (string hashtag in item.hashtags)
                {
                    Hashtag hasht = AddHashtag(hashtag);
                    AddSubjectMessage(tweet, hasht);
                }

                repo.UpdateMessage();
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
        public SubjectMessage AddSubjectMessage(Message msg, Persoon persoon)
        {
            initNonExistingRepo();

            SubjectMessage subjMess = new SubjectMessage()
            {
                Msg = msg,
                Persoon = persoon
            };
            repo.AddSubjectMsg(subjMess);
            return subjMess;
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

            bool ifExists = organisaties.Any(x => x.Naam == naamOrganisatie);

            if (ifExists == true)
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
            bool ifExistsO = organisaties.Any(x => x.Naam == naamOrganisatie);

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
            initNonExistingRepo();

            //Persoon persoon;
            Organisatie organisatie;

            //lijst personen en organisaties opvragen
            IEnumerable<Persoon> personen = repo.ReadPersonen();
            IEnumerable<Organisatie> organisaties = repo.ReadOrganisaties();

            //kijken of persoon en organisatie bestaan
            bool ifExistsP = personen.Any(x => x.Naam == persoon.Naam);
            bool ifExistsO = organisaties.Any(x => x.Naam == naamOrganisatie);

            //kijken of persoon bestaat 
            if (!ifExistsP)
            {
                throw new ArgumentException("Persoon '" + persoon.Naam + "' not found!");
            }
            //kijken of organisatie bestaat & initialiseren
            if (ifExistsO)
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
            initNonExistingRepo();

            StreamReader r = new StreamReader(pathToJson);
            string json = r.ReadToEnd();
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
                } catch
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

        public int CountSubjMsgsPersoon(Onderwerp onderwerp)
        {
            initNonExistingRepo();

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
            initNonExistingRepo();

            IEnumerable<Message> messages = repo.ReadMessages(true);

            return messages;
        }

        public Persoon GetPersoon(int persoonId)
        {
            initNonExistingRepo();

            Persoon persoon = repo.ReadPersoon(persoonId);
            return persoon;
        }

        public Organisatie GetOrganisatie(int organisatieId)
        {
            initNonExistingRepo();

            Organisatie organisatie = repo.ReadOrganisatie(organisatieId);
            return organisatie;
        }

        public IEnumerable<Persoon> GetPersonen()
        {
            initNonExistingRepo();
            IEnumerable<Persoon> personen = repo.ReadPersonen();
            return personen;
        }

        public IEnumerable<Organisatie> GetOrganisaties()
        {
            initNonExistingRepo();
            IEnumerable<Organisatie> organisaties = repo.ReadOrganisaties();
            return organisaties;
        }

        public void ChangeOrganisation(Organisatie organisatie)
        {
            initNonExistingRepo();
            repo.EditOrganisation(organisatie);
        }

        public void ChangePersoon(Persoon persoon)
        {
            initNonExistingRepo();
            repo.EditPersoon(persoon);
        }

        public Persoon GetPersoon(string naam)
        {
            initNonExistingRepo();

            IEnumerable<Persoon> personen = repo.ReadPersonen();
            Persoon persoon = personen.FirstOrDefault(p => p.Naam == naam);

            return persoon;
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

        

        public class zscore
        {
            private string politician;
            private double score;

            public zscore(string politician, double score)
            {
                this.politician = politician;
                this.score = score;
            }
            public override string ToString()
            {
                return "Politieker " + politician + "\n \t score: " + score;
            }
        }



        public void GetAlerts()
        {
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<zscore> zscores = new List<zscore>();
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
                    System.Diagnostics.Debug.WriteLine(start);
                } while (start <= laatsteTweet);
                double totaal = 0;
                foreach (int i in tweetsPerDag)
                {
                    totaal = totaal + i;
                }
                System.Diagnostics.Debug.WriteLine("got here 1");
                System.Diagnostics.Debug.WriteLine("1 " + totaal);
                gemiddelde = totaal / tweetsPerDag.Count();
                System.Diagnostics.Debug.WriteLine("1.1 " + gemiddelde + " " + tweetsPerDag.Count());
                //tweetsPerDag.ForEach(i => System.Diagnostics.Debug.Write("{0}\n", i));
                System.Diagnostics.Debug.WriteLine("got here 2");

                double average = tweetsPerDag.Average();
                System.Diagnostics.Debug.WriteLine(average);
                double sumOfSquaresOfDifferences = tweetsPerDag.Select(val => (val - average) * (val - average)).Sum();
                double sd = Math.Sqrt(sumOfSquaresOfDifferences / tweetsPerDag.Count());
                System.Diagnostics.Debug.WriteLine("got here 3");

                System.Diagnostics.Debug.WriteLine("2 " + sd);

                zscores.Add(new zscore(s, (tweetsPerDag.Last() - gemiddelde) / sd));
                System.Diagnostics.Debug.WriteLine((((double)tweetsPerDag.Last() - gemiddelde) / (gemiddelde * 100)));
                System.Diagnostics.Debug.WriteLine(tweetsPerDag.Last());
                System.Diagnostics.Debug.WriteLine(gemiddelde);
                System.Diagnostics.Debug.WriteLine("---");
                System.Diagnostics.Debug.WriteLine(tweetsPerDag.Count());
                System.Diagnostics.Debug.WriteLine(totaal);
                System.Diagnostics.Debug.WriteLine(s);
            }
            System.Diagnostics.Debug.WriteLine("---");
            foreach (zscore z in zscores)
            {
                System.Diagnostics.Debug.WriteLine(z.ToString());
            }

            System.Diagnostics.Debug.WriteLine("got here 4");

            //GetAlerts();
            //SendMail();
            //GetNumber(repo.ReadPersonen().ToList().Where(p => p.Naam == "Jan Jambon").First());
            GetTweetsPerDag(repo.ReadPersonen().ToList().Where(p => p.Naam == "Jan Jambon").First());
            System.Diagnostics.Debug.WriteLine(repo.ReadMessages().ToList().Count());
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
                System.Diagnostics.Debug.WriteLine("Mail says no");
            }
        }

        //public Dictionary<Persoon, double> GetRanking(int aantal, int interval_uren, bool puntNotatie = true)
        public List<GraphData> GetRanking(int aantal, int interval_uren, bool puntNotatie = false)
        
            {
            initNonExistingRepo();
            List<Persoon> personen = repo.ReadPersonen().ToList();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            int laatstePeriode;
            int voorlaatstePeriode;
                //Sam
                //Dictionary<Persoon, double> ranking = new Dictionary<Persoon, double>();
                List<GraphData> ranking = new List<GraphData>();
                foreach (Persoon p in personen)
            {
                int teller = messages.Where(m => m.IsFrom(p)).Count();
                List<Message> messages2 = messages.Where(m => m.IsFrom(p)).ToList();
                laatstePeriode = messages2.Where(m => lastTweet.AddHours(interval_uren * -1) < m.Date).Count();
                voorlaatstePeriode = messages2.Where(m => lastTweet.AddHours((interval_uren * 2) * -1) < m.Date && m.Date < lastTweet.AddHours(interval_uren * -1)).Count();
                if (puntNotatie == true)
                {
                    ranking.Add(new GraphData(p.Naam, (int)CalculateChange(voorlaatstePeriode, laatstePeriode)));
                }
                else
                {
                    ranking.Add(new GraphData(p.Naam, (int)CalculateChange(voorlaatstePeriode, laatstePeriode) * 100));
                }
                /*if (laatstePeriode != 0 && voorlaatstePeriode != 0)
                {
                    ranking.Add(p, ((laatstePeriode - voorlaatstePeriode) / voorlaatstePeriode) * 100);
                }*/
            }

            foreach (var v in ranking)
            {
                //System.Diagnostics.Debug.WriteLine(v.Key.Naam + " " + v.Value);
            }

                ranking = ranking.OrderByDescending(r => r.value1).ToList();
            return ranking.GetRange(0, aantal);
            }

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
            initNonExistingRepo();
            List<Message> messages = repo.ReadMessages(true).ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            int aantal;

            if (laatsteAantalUren == 0)
            {
                aantal = messages.Where(m => m.IsFrom(persoon)).Count();
            }
            else
            {
                aantal = messages.Where(m => m.IsFrom(persoon) && m.Date > lastTweet.AddHours(laatsteAantalUren * -1)).Count();
            }

            return aantal;
        }
        //Sam
        //public Dictionary<DateTime, int> GetTweetsPerDag(Persoon persoon, int aantalDagenTerug = 0)
            public List<GraphData> GetTweetsPerDag(Persoon persoon, int aantalDagenTerug = 0)
        {
            initNonExistingRepo();
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
            List<GraphData> GraphDataList = new List<GraphData>();
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
            for (int i = 0; i < aantalDagenTerug+1; i++)
            {
                //Sam
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                //Sam
                GraphData graph = new GraphData(date, messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFrom(persoon)).Count());
                
                GraphDataList.Add(graph);
                lastTweet = lastTweet.AddDays(-1);
            }
            
            foreach (var v in GraphDataList)
            {
                System.Diagnostics.Debug.WriteLine(v.label + " " + v.value1);
            }
            

            return GraphDataList;
        }

        public List<GraphData2> GetTweetsPerDag2(Persoon persoon1, Persoon persoon2, Persoon persoon3, Persoon persoon4, Persoon persoon5, int aantalDagenTerug = 0)
        {
            initNonExistingRepo();
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
                GraphDataList.Add(new GraphData2(date, messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFrom(persoon1)).Count(), messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFrom(persoon2)).Count(), messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFrom(persoon3)).Count(), messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFrom(persoon4)).Count(), messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFrom(persoon5)).Count()));
                lastTweet = lastTweet.AddDays(-1);
            }

            foreach (var v in GraphDataList)
            {
                System.Diagnostics.Debug.WriteLine(v.label + " " + v.value1);
            }


            return GraphDataList;
        }







        public List<GraphData> GetTweetsPerDag(Persoon persoon, Gebruiker user, int aantalDagenTerug = 0)
        {
            initNonExistingRepo(true);

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

            Dictionary<DateTime, int> tweetsPerDag = new Dictionary<DateTime, int>();

            //======= Edit : 10 mei 2018 : Stephane ======//

            //bool UoW = false;
            //repo.setUnitofWork(UoW);

            //Domain.Dashboard.DashItem dashItem = dashMgr.AddDashItem(user, persoon);
            //dashItem.Graphdata = new Collection<GraphData>();

            //=======                              =======//

            //Sam
            List<GraphData> GraphDataList = new List<GraphData>();
            for (int i = 0; i < aantalDagenTerug + 1; i++)
            {
                //Sam
                string date = lastTweet.Date.Year + "-" + lastTweet.Date.Month + "-" + lastTweet.Date.Day;
                //Sam

                //======= Edit : 10 mei 2018 : Stephane ======//
                
                GraphData graph = new GraphData(date, messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFrom(persoon)).Count());
                dashMgr.AddGraph(graph);
                //dashItem.Graphdata.Add(graph);
                //dashMgr.UpdateDashItem(dashItem);

                //UoW = true;
                //repo.setUnitofWork(UoW);

                //=======                              =======//

                GraphDataList.Add(graph);
                lastTweet = lastTweet.AddDays(-1);
            }
            uowManager.Save();
            return GraphDataList;
        }
    }
}