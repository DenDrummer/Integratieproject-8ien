using IP3_8IEN.BL.Domain.Data;
using IP3_8IEN.BL.Domain.Gebruikers;
using Newtonsoft.Json;
using System.IO;

using IP3_8IEN.DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL
{
    public class GebruikerManager : IGebruikerManager
    {
        private UnitOfWorkManager uowManager;
        private IGebruikerRepository repo;
        private IDataManager dataMgr;
        private IDashManager dashMgr;

        // Deze constructor gebruiken we voor operaties binnen de package
        public GebruikerManager()
        {
            //repo = new GebruikerRepository();
        }

        // We roepen deze constructor aan wanneer we met twee repositories gaan werken
        public GebruikerManager(UnitOfWorkManager uofMgr)
        {
            uowManager = uofMgr;
            repo = new GebruikerRepository(uowManager.UnitOfWork);
        }

        ////inladen vanuit json formaat
        //public void AddApplicationGebruikers(string filePath)
        //{
        //    initNonExistingRepo();

        //    appUserMgr = new ApplicationUserManager();

        //    //sourceUrl /relatief path
        //    StreamReader r = new StreamReader(filePath);
        //    string json = r.ReadToEnd();
        //    List<Message> gebruikers = new List<Message>();

        //    dynamic users = JsonConvert.DeserializeObject(json);

        //    foreach (var item in users.records)
        //    {
        //        Domain.ApplicationUser gebruiker = new Domain.ApplicationUser()
        //        {
        //            UserName = item.Username,
        //            VoorNaam = item.Voornaam,
        //            AchterNaam = item.Achternaam,
        //            Email = item.email,
        //            Geboortedatum = item.Geboortedatum
        //        };
        //        string passw = item.Password;
        //        appUserMgr.CreateAsync(gebruiker, passw);
        //    }
        //}

        //inladen vanuit json formaat
        //public void AddGebruikers(string filePath)
        //{
        //    initNonExistingRepo();

        //    //sourceUrl /relatief path
        //    StreamReader r = new StreamReader(filePath);
        //    string json = r.ReadToEnd();

        //    dynamic users = JsonConvert.DeserializeObject(json);

        //    foreach (var item in users.records)
        //    {
        //        Gebruiker gebruiker = new Gebruiker()
        //        {
        //            Username = item.Username,
        //            Voornaam = item.Voornaam,
        //            Naam = item.Achternaam,
        //            Email = item.email,
        //            Geboortedatum = item.Geboortedatum
        //        };
        //        repo.AddingGebruiker(gebruiker);
        //    }
        //}

        // We zoeken een gebruiker op basis van 'Username'
        public Gebruiker FindUser(string username)
        {
            initNonExistingRepo();

            IEnumerable<Gebruiker> users = repo.ReadGebruikers();
            Gebruiker user = users.FirstOrDefault(x => x.Username == username);
            return user;
        }
        public void DeleteGebruiker(string username)
        {
            initNonExistingRepo();
            IEnumerable<Gebruiker> users = repo.ReadGebruikers();
            Gebruiker user = users.FirstOrDefault(x => x.Username == username);
            repo.DeleteGebruiker(user);

        }

        public IEnumerable<Gebruiker> GetGebruikers()
        {
            initNonExistingRepo();
            return repo.ReadGebruikers();
        }

        // Hier werken we met 'Unit of Work'
        // omdat we informatie uit de data package nodig hebben
        public void AddAlertInstelling(string filePath)
        {
            initNonExistingRepo(true);

            //sourceUrl /relatief path
            StreamReader r = new StreamReader(filePath);
            string json = r.ReadToEnd();
            List<Message> alertConfigs = new List<Message>();

            dynamic alertInstellingen = JsonConvert.DeserializeObject(json);

            string user = null;
            bool notificationWeb;
            bool email;
            bool mobileNotification;
            bool state;
            int onderwerpId;
            int onderwerpId2;
            int thresh;
            bool negatief;


            dataMgr = new DataManager(uowManager);
            //We laten de transactie eve denken dat we geen 'UoW' gebruiken zodat er niet
            //van repo gewisseld wordt bij het aanroepen van een nieuwe methode
            bool UoW = false;
            repo.setUnitofWork(UoW);

            IEnumerable<Onderwerp> onderwerpen = dataMgr.ReadOnderwerpen();

            foreach (var item in alertInstellingen.records)
            {
                if (item.Threshold != null) {
                    ValueFluctuation vf = new ValueFluctuation()
                    {
                        Gebruiker = FindUser((String)item.Username),
                        NotificationWeb = (bool)item.NotificationWeb,
                        Email = (bool)item.Email,
                        MobileNotification = (bool)item.MobileNotification,
                        AlertState = true,
                        Onderwerp = onderwerpen.FirstOrDefault(x => x.OnderwerpId == (int)item.OnderwerpId),
                        ThresholdValue = item.Threshold
                    };
                    repo.AddingAlertInstelling(vf);
                    System.Diagnostics.Debug.WriteLine("jah");
                }else if (item.OnderwerpId2 != null) {
                    HogerLager hl = new HogerLager()
                    {
                        Gebruiker = FindUser((String)item.Username),
                        NotificationWeb = (bool)item.NotificationWeb,
                        Email = (bool)item.Email,
                        MobileNotification = (bool)item.MobileNotification,
                        AlertState = true,
                        Onderwerp = onderwerpen.FirstOrDefault(x => x.OnderwerpId == (int)item.OnderwerpId),
                        Onderwerp2 = onderwerpen.FirstOrDefault(x => x.OnderwerpId == (int)item.OnderwerpId2)
                    };
                    repo.AddingAlertInstelling(hl);
                    System.Diagnostics.Debug.WriteLine("jah");
                }else
                { 
                    PositiefNegatief pn = new PositiefNegatief()
                    {
                        Gebruiker = FindUser((String)item.Username),
                        NotificationWeb = (bool)item.NotificationWeb,
                        Email = (bool)item.Email,
                        MobileNotification = (bool)item.MobileNotification,
                        AlertState = true,
                        Onderwerp = onderwerpen.FirstOrDefault(x => x.OnderwerpId == (int)item.OnderwerpId),
                        negatief = (bool)item.Negatief
                    };
                    repo.AddingAlertInstelling(pn);
                    System.Diagnostics.Debug.WriteLine("jah");
                }
                
                
                uowManager.Save();
            }
            //we zetten 'UoW' boolian terug op true
            UoW = true;
            repo.setUnitofWork(UoW);
        }

        // We initialiseren een 'Alert' met het toewijzen van een 'AlertInstelling' adhv een 'Id' 
        // ook voegen het moment van creatie toe ('CreatedOn')
        // 'AlertContent' kan een string zijn met informatie om te verzenden naar een gebruiker


        public void AddAlert(string alertContent, int alertInstellingId)
        {
            initNonExistingRepo();

            IEnumerable<AlertInstelling> fluctuations = repo.ReadValueFluctuations();
            List<AlertInstelling> Ais = fluctuations.ToList();
            Ais.AddRange(repo.ReadHogerLagers().ToList());
            Ais.AddRange(repo.ReadPositiefNegatiefs().ToList());

            AlertInstelling ai = Ais.FirstOrDefault(v => v.AlertInstellingId == alertInstellingId);
            Alert alert = new Alert()
            {
                AlertContent = alertContent,
                AlertInstelling = ai,
                CreatedOn = DateTime.Now
            };
            //alert toevoegen aan de ICollection van 'AlertInstelling'
            var alertColl = ai.Alerts;
            if (alertColl != null)
            {
                ai.Alerts = alertColl.ToList();
            }
            else
            {
                ai.Alerts = new Collection<Alert>();
            }

            ai.Alerts.Add(alert);

            //eerst alert creëren zodat deze een PK toegewegen krijgt
            repo.AddingAlert(alert);
            //dan de AlertInstelling updaten met de nieuwe 'Alert'
            repo.UpdateAlertInstelling(ai);
        }


        // Alerts inlezen via json bestand
        public void AddAlerts(string filePath)
        {
            initNonExistingRepo();

            StreamReader r = new StreamReader(filePath);
            string json = r.ReadToEnd();
            List<Message> alertList = new List<Message>();

            dynamic Alerts = JsonConvert.DeserializeObject(json);

            string alertContent;
            int alertInstellingId;

            foreach (var item in Alerts.records)
            {
                alertContent = item.AlertContent;
                alertInstellingId = item.AlertInstellingId;

                AddAlert(alertContent, alertInstellingId);
            };
        }

        public IEnumerable<Alert> GetAlerts()
        {
            initNonExistingRepo();
            return repo.ReadAlerts();
        }

        public Alert GetAlert(int alertId)
        {
            initNonExistingRepo();

            Alert alert = repo.ReadAlert(alertId);
            return alert;
        }

        public void AddGebruiker(string userName, string userId, string naam, string voornaam, string role = "User")
        {
            initNonExistingRepo();

            Gebruiker gebruiker = new Gebruiker
            {
                GebruikerId = userId,
                Username = userName,
                Voornaam = voornaam,
                Naam = naam,
                Role = role,
                Active = true
            };
            repo.AddingGebruiker(gebruiker);

            dashMgr = new DashManager();

            //Dashboard initialiseren voor nieuwe gebruiker en opvullen met vaste grafieken
            dashMgr.InitializeDashbordNewUsers(gebruiker.GebruikerId);
        }

        public void UpdateGebruiker(Gebruiker gebruiker)
        {
            initNonExistingRepo();

            repo.UpdateGebruiker(gebruiker);
        }
        
        public void DeleteUser(string userId)
        {
            initNonExistingRepo();

            //IdentityUser wordt verwijderd, data gebruiker wordt overschreven
            Gebruiker user = repo.ReadGebruikers().FirstOrDefault(u => u.GebruikerId == userId);

            user.Username = "Deleted";
            user.Naam = "Deleted";
            user.Voornaam = "Deleted";
            user.Email = "Deleted";
            user.Geboortedatum = DateTime.Now;
            user.Role = "User";
            //We geven aan dat de account 'inactive' is
            user.Active = false;

            UpdateGebruiker(user);
        }

        public IEnumerable<Gebruiker> GetUsers()
        {
            initNonExistingRepo();

            return repo.ReadUsers();
        }

        public IEnumerable<ApplicationUser> GetUsersInRoles(IEnumerable<ApplicationUser> appUsers, string role)
        {
            initNonExistingRepo();

            List<ApplicationUser> appUsersInRole = new List<ApplicationUser>();
            IEnumerable<Gebruiker> users = repo.ReadGebruikers().Where(u => u.Role == role && u.Active == true);
            foreach(Gebruiker user in users)
            {
                appUsersInRole.Add(appUsers.FirstOrDefault(u => u.Id == user.GebruikerId));
            }
            return appUsersInRole;
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
                repo = new DAL.GebruikerRepository(uowManager.UnitOfWork);
            }
            // Als we niet met UoW willen werken, dan maken we een repo aan als die nog niet bestaat.
            else
            {
                //zien of repo al bestaat
                if (repo == null)
                {
                    repo = new DAL.GebruikerRepository();
                }
                else
                {
                    //checken wat voor repo we hebben
                    bool isUoW = repo.isUnitofWork();
                    if (isUoW)
                    {
                        repo = new DAL.GebruikerRepository();
                    }
                    else
                    {
                        // repo behoudt zijn context
                    }
                }
            }
        }


        public void GetAlertHogerLagers()
        {
            System.Diagnostics.Debug.WriteLine("HL started");
            initNonExistingRepo();
            dataMgr = new DataManager();

            List<HogerLager> hogerLagers = repo.ReadHogerLagers().ToList();

            foreach(HogerLager hl in hogerLagers)
            {
                //Check if onderwerp is een peroon
                if (hl.Onderwerp is Persoon && hl.Onderwerp2 is Persoon)
                {
                    if (hl.OneHigherThanTwo)
                    {
                        if (CalculateZscore(hl.Onderwerp) < CalculateZscore(hl.Onderwerp2))
                        {
                            Persoon p1 = (Persoon)hl.Onderwerp;
                            Persoon p2 = (Persoon)hl.Onderwerp2;
                            repo.AddingAlert(new Alert()
                            {
                                AlertContent = p2.Naam + "is nu populairder dan " + p1.Naam,
                                AlertInstelling = hl,
                                CreatedOn = DateTime.Now
                            });
                            System.Diagnostics.Debug.WriteLine("One HL added");
                        }
                    }
                    else
                    {
                        if (CalculateZscore(hl.Onderwerp) > CalculateZscore(hl.Onderwerp2))
                        {
                            Persoon p1 = (Persoon)hl.Onderwerp;
                            Persoon p2 = (Persoon)hl.Onderwerp2;
                            repo.AddingAlert(new Alert()
                            {
                                AlertContent = p1.Naam + "is nu populairder dan " + p2.Naam,
                                AlertInstelling = hl,
                                CreatedOn = DateTime.Now
                            });
                            System.Diagnostics.Debug.WriteLine("One HL added");
                        }
                    }
                }
                //als onderwerp een organistatie is
                else
                {
                    if (hl.OneHigherThanTwo)
                    {
                        if (CalculateZscore(hl.Onderwerp) < CalculateZscore(hl.Onderwerp2))
                        {
                            Organisatie o1 = (Organisatie)hl.Onderwerp;
                            Organisatie o2 = (Organisatie)hl.Onderwerp2;
                            repo.AddingAlert(new Alert()
                            {
                                AlertContent = o2.Afkorting + "is nu populairder dan " + o1.Afkorting,
                                AlertInstelling = hl,
                                CreatedOn = DateTime.Now
                            });
                            System.Diagnostics.Debug.WriteLine("One HL added");
                        }
                    }
                    else
                    {
                        if (CalculateZscore(hl.Onderwerp) > CalculateZscore(hl.Onderwerp2))
                        {
                            Organisatie o1 = (Organisatie)hl.Onderwerp;
                            Organisatie o2 = (Organisatie)hl.Onderwerp2;
                            repo.AddingAlert(new Alert()
                            {
                                AlertContent = o2.Afkorting + "is nu populairder dan " + o2.Afkorting,
                                AlertInstelling = hl,
                                CreatedOn = DateTime.Now
                            });
                            System.Diagnostics.Debug.WriteLine("One HL added");
                        }
                    }
                }
            }
        }
        
        public void GetAlertValueFluctuations()
        {
            System.Diagnostics.Debug.WriteLine("VF Started");
            initNonExistingRepo();
            dataMgr = new DataManager();

            List<ValueFluctuation> valueFluctuations = repo.ReadValueFluctuations().ToList();
            List<Message> messages = dataMgr.ReadMessagesWithSubjMsgs().ToList();

            foreach(ValueFluctuation vf in valueFluctuations)
            {
                if (vf.Onderwerp is Persoon)
                {
                    if (messages.Where(m => m.IsFromPersoon((Persoon)vf.Onderwerp) && m.Date.Date == DateTime.Now.Date).Count() > vf.CurrentValue + vf.ThresholdValue)
                    {
                        Persoon p = (Persoon)vf.Onderwerp;
                        repo.AddingAlert(new Alert()
                        {

                            AlertContent = "Thresholdvalue voor " + p.Naam + " is overschreden",
                            AlertInstelling = vf,
                            CreatedOn = DateTime.Now
                        });
                        System.Diagnostics.Debug.WriteLine("One VF added");
                    }
                }
                else
                {
                    if (messages.Where(m => m.IsFromOrganisatie((Organisatie)vf.Onderwerp) && m.Date.Date == DateTime.Now.Date).Count() > vf.CurrentValue + vf.ThresholdValue)
                    {
                        Organisatie o = (Organisatie)vf.Onderwerp;
                        repo.AddingAlert(new Alert()
                        {

                            AlertContent = "Thresholdvalue voor " + o.Afkorting + " is overschreden",
                            AlertInstelling = vf,
                            CreatedOn = DateTime.Now
                        });
                        System.Diagnostics.Debug.WriteLine("One VF added");
                    }
                }
            }
        }

        public void GetAlertPositiefNegatiefs()
        {
            System.Diagnostics.Debug.WriteLine("PN started");
            initNonExistingRepo();
            dataMgr = new DataManager();
            double total = 1;

            List<PositiefNegatief> positiefNegatiefs = repo.ReadPositiefNegatiefs().ToList();
            List<Message> messages = dataMgr.ReadMessagesWithSubjMsgs().ToList();

            foreach (PositiefNegatief pn in positiefNegatiefs)
            {
                if (pn.Onderwerp is Persoon)
                {
                    messages = messages.Where(m => m.IsFromPersoon((Persoon)pn.Onderwerp)).ToList();
                    total = messages.Sum(m => m.Polarity);

                    if (pn.negatief == true)
                    {
                        if (total / messages.Count() > 0)
                        {
                            Persoon p = (Persoon)pn.Onderwerp;
                            repo.AddingAlert(new Alert()
                            {
                                AlertContent = p.Naam + " is nu positief",
                                AlertInstelling = pn,
                                CreatedOn = DateTime.Now
                            });
                            System.Diagnostics.Debug.WriteLine("One PN added");
                        }
                    }
                    else
                    {
                        if (total / messages.Count() < 0)
                        {
                            Persoon p = (Persoon)pn.Onderwerp;
                            repo.AddingAlert(new Alert()
                            {
                                AlertContent = p.Naam + " is nu negatief",
                                AlertInstelling = pn,
                                CreatedOn = DateTime.Now
                            });
                            System.Diagnostics.Debug.WriteLine("One PN added");
                        }
                    }
                }
                else
                {
                    messages = messages.Where(m => m.IsFromOrganisatie((Organisatie)pn.Onderwerp)).ToList();
                    total = messages.Sum(m => m.Polarity);

                    if (pn.negatief == true)
                    {
                        if (total / messages.Count() > 0)
                        {
                            Organisatie o = (Organisatie)pn.Onderwerp;
                            repo.AddingAlert(new Alert()
                            {
                                AlertContent = o.Afkorting + " is nu positief",
                                AlertInstelling = pn,
                                CreatedOn = DateTime.Now
                            });
                            System.Diagnostics.Debug.WriteLine("One PN added");
                        }
                    }
                    else
                    {
                        if (total / messages.Count() < 0)
                        {
                            Organisatie o = (Organisatie)pn.Onderwerp;
                            repo.AddingAlert(new Alert()
                            {
                                AlertContent = o.Afkorting + " is nu negatief",
                                AlertInstelling = pn,
                                CreatedOn = DateTime.Now
                            });
                            System.Diagnostics.Debug.WriteLine("One PN added");
                        }
                    }
                }
            }

        }

        double CalculateZscore(Onderwerp onderwerp)
        {
            initNonExistingRepo();
            int totaalTweets = 0;
            //totaalTweets = messages.Where(Message => Message.Politician == s).Count();
            bool test;
            List<Message> messages = dataMgr.ReadMessagesWithSubjMsgs().ToList();
            List<Message> ms = new List<Message>();
            List<int> tweetsPerDag = new List<int>();
            double gemiddelde;
            DateTime laatsteTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;

            if (onderwerp is Persoon)
            {
                Persoon p = (Persoon)onderwerp;
                foreach (Message m in messages)
                {
                    test = false;
                    foreach (SubjectMessage sm in m.SubjectMessages)
                    {
                        if (sm.Persoon != null && sm.Persoon.Naam == p.Naam)
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
                System.Diagnostics.Debug.WriteLine(average);
                double sumOfSquaresOfDifferences = tweetsPerDag.Select(val => (val - average) * (val - average)).Sum();
                double sd = Math.Sqrt(sumOfSquaresOfDifferences / tweetsPerDag.Count());

                double test2 = ((tweetsPerDag.Last() - gemiddelde) / sd);
                return test2;
            }
            else
            {
                Organisatie o = (Organisatie)onderwerp;
                foreach (Message m in messages)
                {
                    test = false;
                    foreach (SubjectMessage sm in m.SubjectMessages)
                    {
                        bool test3 = false;
                        foreach (Tewerkstelling t in sm.Persoon.Tewerkstellingen)
                        {
                            if(t.Organisatie.Afkorting == o.Afkorting)
                            {
                                test3 = true;
                            }
                        }
                        if (sm.Persoon != null && test3)
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
                System.Diagnostics.Debug.WriteLine(average);
                double sumOfSquaresOfDifferences = tweetsPerDag.Select(val => (val - average) * (val - average)).Sum();
                double sd = Math.Sqrt(sumOfSquaresOfDifferences / tweetsPerDag.Count());

                double test2 = ((tweetsPerDag.Last() - gemiddelde) / sd);
                return test2;
            }
            
        }

        public void WeeklyReview()
        {
            initNonExistingRepo();
            List<Gebruiker> gebruikers = new List<Gebruiker>();
            gebruikers = repo.ReadGebruikersWithAlertInstellingen().ToList();
            List<Alert> dezeWeek = new List<Alert>();
            StringBuilder sb = new StringBuilder();

            foreach (Gebruiker g in gebruikers)
            {
                sb.Clear();
                sb.Append(@"<div id=""wrapper"" style=""width:600px;margin:0 auto; border:1px solid black; 
                            overflow:hidden; padding: 10px 10px 10px 10px;"" ><p><i>");
                // Voor- en Achternaam kunnen voorlopig leeg zijn
                //sb.Append(g.Voornaam + " " + g.Naam);
                sb.Append(g.Username);
                sb.Append(@", </i></p>
                            <p>Via de Weekly Review wordt u op de hoogte gehouden van alle trending Onderwerpen die </br>
                            u volgt. Indien u op de hoogte gehouden wilt worden van nog meer onderwerpen, kan u 
                            </br> steeds extra onderwerpen volgen op <a href=""www.8ien.be""> Weekly Reviews </a>. </p>
                            <h3>Personen</h3> <div style=""margin: 0px;""> <p>Naam : Bart De Wever </p> <ul>");
                if (g.AlertInstellingen != null) {
                foreach (AlertInstelling al in g.AlertInstellingen)
                {
                    if (al.Alerts != null) {
                    foreach (Alert a in al.Alerts)
                    {
                        if (DatesAreInTheSameWeek(a.CreatedOn, DateTime.Now))
                        {
                            dezeWeek.Add(a);
                                    sb.Append("<li>" +  a.ToString() + "</li>");
                        }
                    }
                    }
                }
                }
                sb.Append(@"</ul></div></div>");
                SendMail(dezeWeek, g.Email, sb.ToString());
            }
         }

        public List<HogerLager> GetHogerLagersByUser()
        {
            initNonExistingRepo();

            List<HogerLager> hogerLagers = repo.ReadHogerLagers().ToList();
            //hogerLagers = hogerLagers.Where(hl => hl.Gebruiker == gebruiker).ToList();

            return hogerLagers;
        }

        public List<ValueFluctuation> GetValueFluctuationsByUser()
        {
            initNonExistingRepo();

            List<ValueFluctuation> valueFluctuations = repo.ReadValueFluctuations().ToList();
            //valueFluctuations = valueFluctuations.Where(vf => vf.Gebruiker == gebruiker).ToList();

            return valueFluctuations;
        }

        public List<PositiefNegatief> GetPositiefNegatiefsByUser()
        {
            initNonExistingRepo();

            List<PositiefNegatief> positiefNegatiefs = repo.ReadPositiefNegatiefs().ToList();
            //positiefNegatiefs = positiefNegatiefs.Where(pn => pn.Gebruiker == gebruiker).ToList();

            return positiefNegatiefs;
        }

        public List<Alert> GetAlertsByUser(Gebruiker gebruiker)
        {
            initNonExistingRepo();

            List<Alert> alerts = repo.ReadAlerts().ToList();
            alerts = alerts.Where(a => a.AlertInstelling.Gebruiker == gebruiker).ToList();

            return alerts;
        }

        bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

            return d1 == d2;
        }
        void SendMail(List<Alert> alerts, string email, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("integratieproject.8ien@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Test";
                mail.Body = body;
                mail.IsBodyHtml = true;

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

        //SendMail(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        public void SendMail(string userId, string subject, string body)
        {
            try
            {
                string userEmail = repo.ReadGebruiker(userId).Email;

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("integratieproject.8ien@gmail.com");
                mail.To.Add(userEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

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
    }
}