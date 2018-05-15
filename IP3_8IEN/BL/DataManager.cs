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
                        since = "28 Apr 2018 0:01",
                        //until weglaten --> last scraping
                        //until = "26 Apr 2018 23:59",
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


            foreach (var person in persons)
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

        public Dictionary<Persoon, double> GetRanking(int aantal, int interval_uren, bool puntNotatie = true)
        {
            initNonExistingRepo();
            List<Persoon> personen = repo.ReadPersonen().ToList();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            DateTime lastTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;
            int laatstePeriode;
            int voorlaatstePeriode;

            Dictionary<Persoon, double> ranking = new Dictionary<Persoon, double>();

            foreach (Persoon p in personen)
            {
                int teller = messages.Where(m => m.IsFromPersoon(p)).Count();
                List<Message> messages2 = messages.Where(m => m.IsFromPersoon(p)).ToList();
                laatstePeriode = messages2.Where(m => lastTweet.AddHours(interval_uren * -1) < m.Date).Count();
                voorlaatstePeriode = messages2.Where(m => lastTweet.AddHours((interval_uren * 2) * -1) < m.Date && m.Date < lastTweet.AddHours(interval_uren * -1)).Count();
                if (puntNotatie == true)
                {
                    ranking.Add(p, CalculateChange(voorlaatstePeriode, laatstePeriode));
                }
                else
                {
                    ranking.Add(p, CalculateChange(voorlaatstePeriode, laatstePeriode) * 100);
                }
                /*if (laatstePeriode != 0 && voorlaatstePeriode != 0)
                {
                    ranking.Add(p, ((laatstePeriode - voorlaatstePeriode) / voorlaatstePeriode) * 100);
                }*/
            }

            foreach (var v in ranking)
            {
                System.Diagnostics.Debug.WriteLine(v.Key.Naam + " " + v.Value);
            }

            return ranking;
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
            List<Message> messages = repo.ReadMessages().ToList();
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

        public Dictionary<DateTime, int> GetTweetsPerDag(Persoon persoon, int aantalDagenTerug = 0)
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

            do
            {
                tweetsPerDag.Add(lastTweet.Date, messages.Where(m => m.Date.Date == lastTweet.Date && m.IsFromPersoon(persoon)).Count());
                lastTweet = lastTweet.AddDays(-1);
            } while (lastTweet >= stop);

            foreach (var v in tweetsPerDag)
            {
                System.Diagnostics.Debug.WriteLine(v.Key + " " + v.Value);
            }
            return tweetsPerDag;
        }

        public double GetPolarityByPerson(Persoon persoon)
        {
            initNonExistingRepo();
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
            initNonExistingRepo();
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
            initNonExistingRepo();
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
            initNonExistingRepo();
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
            initNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            double totalObjectivity = 0;

            messages = messages.Where(m => m.IsFromPersoon(persoon) && m.Date > start).ToList();

            foreach (Message m in messages)
            {
                totalObjectivity += m.Objectivity;
            }

            return totalObjectivity / messages.Count;
        }

        public double GetObjectivityByPerson(Persoon persoon,DateTime start, DateTime stop)
        {
            initNonExistingRepo();
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
            initNonExistingRepo();
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
            initNonExistingRepo();
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
            initNonExistingRepo();
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
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data;
        }

        public List<GraphData> GetTopWordsCount(int aantal)
        {
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data.GetRange(0,aantal);
        }

        public List<GraphData> GetTopWordsCount(int aantal, DateTime start)
        {
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data.GetRange(0, aantal);
        }

        public List<GraphData> GetTopWordsCount(int aantal, DateTime start, DateTime stop)
        {
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data.GetRange(0, aantal);
        }

        public List<GraphData> GetTopWordsCount(DateTime start, DateTime stop)
        {
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data;
        }

        public int GetWordCountByName(string name)
        {
            initNonExistingRepo();
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
            initNonExistingRepo();
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
            initNonExistingRepo();
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

        public List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2)
        {
            initNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData> data = new List<GraphData>();

            data.Add(new GraphData(p1.Naam, messages.Where(m => m.IsFromPersoon(p1)).Count()));
            data.Add(new GraphData(p2.Naam, messages.Where(m => m.IsFromPersoon(p2)).Count()));

            return data;
        }

        public List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2, Persoon p3)
        {
            initNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData> data = new List<GraphData>();

            data.Add(new GraphData(p1.Naam, messages.Where(m => m.IsFromPersoon(p1)).Count()));
            data.Add(new GraphData(p2.Naam, messages.Where(m => m.IsFromPersoon(p2)).Count()));
            data.Add(new GraphData(p3.Naam, messages.Where(m => m.IsFromPersoon(p3)).Count()));

            return data;
        }

        public List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2, Persoon p3, Persoon p4)
        {
            initNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData> data = new List<GraphData>();

            data.Add(new GraphData(p1.Naam, messages.Where(m => m.IsFromPersoon(p1)).Count()));
            data.Add(new GraphData(p2.Naam, messages.Where(m => m.IsFromPersoon(p2)).Count()));
            data.Add(new GraphData(p3.Naam, messages.Where(m => m.IsFromPersoon(p3)).Count()));
            data.Add(new GraphData(p4.Naam, messages.Where(m => m.IsFromPersoon(p4)).Count()));

            return data;
        }

        public List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2, Persoon p3,Persoon p4,Persoon p5)
        {
            initNonExistingRepo();
            List<Message> messages = ReadMessagesWithSubjMsgs().ToList();
            List<GraphData> data = new List<GraphData>();

            data.Add(new GraphData(p1.Naam, messages.Where(m => m.IsFromPersoon(p1)).Count()));
            data.Add(new GraphData(p2.Naam, messages.Where(m => m.IsFromPersoon(p2)).Count()));
            data.Add(new GraphData(p3.Naam, messages.Where(m => m.IsFromPersoon(p3)).Count()));
            data.Add(new GraphData(p4.Naam, messages.Where(m => m.IsFromPersoon(p4)).Count()));
            data.Add(new GraphData(p5.Naam, messages.Where(m => m.IsFromPersoon(p5)).Count()));

            return data;
        }

        public List<GraphData> GetTopStoryCount()
        {
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data;
        }

        public List<GraphData> GetTopStoryCount(int aantal)
        {
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data.GetRange(0,aantal);
        }

        public List<GraphData> GetTopStoryCount(int aantal, DateTime start)
        {
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data.GetRange(0,aantal);
        }

        public List<GraphData> GetTopStoryCount(int aantal, DateTime start, DateTime stop)
        {
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data.GetRange(0, aantal);
        }

        public List<GraphData> GetTopStoryCount(DateTime start, DateTime stop)
        {
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data;
        }

        public List<GraphData> GetTopStoryByPolitician(Persoon persoon)
        {
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data;

        }
        public List<GraphData2> GetComparisonPersonNumberOfTweetsOverTime(Persoon p1, Persoon p2, Persoon p3, Persoon p4, Persoon p5)
        {
            initNonExistingRepo();
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
            initNonExistingRepo();
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

            data = data.OrderByDescending(d => d.value).ToList();
            return data.GetRange(0, aantal);
        }
    }
}