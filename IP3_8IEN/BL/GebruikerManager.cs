using IP_8IEN.BL.Domain.Data;
using IP_8IEN.BL.Domain.Gebruikers;
using Newtonsoft.Json;
using System.IO;

using IP_8IEN.DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using IP_8IEN.BL;
using System.Net.Mail;
using System.Text;
using IP3_8IEN.BL.Domain.Gebruikers;

namespace IP_8IEN.BL
{
    public class GebruikerManager : IGebruikerManager
    {
        private UnitOfWorkManager uowManager;
        private IGebruikerRepository repo;
        private IDataManager dataMgr;

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

        //inladen vanuit json formaat
        public void AddGebruikers(string filePath)
        {
            initNonExistingRepo();

            //sourceUrl /relatief path
            StreamReader r = new StreamReader(filePath);
            string json = r.ReadToEnd();
            List<Message> gebruikers = new List<Message>();

            dynamic users = JsonConvert.DeserializeObject(json);

            foreach (var item in users.records)
            {
                Gebruiker gebruiker = new Gebruiker()
                {
                    Username = item.Username,
                    Voornaam = item.Voornaam,
                    Naam = item.Achternaam,
                    Email = item.email,
                    Geboortedatum = item.Geboortedatum
                };
                repo.AddingGebruiker(gebruiker);
            }
        }

        // We zoeken een gebruiker op basis van 'Username'
        public Gebruiker FindUser(string username)
        {
            initNonExistingRepo();

            IEnumerable<Gebruiker> users = repo.ReadGebruikers();
            Gebruiker user = users.FirstOrDefault(x => x.Username == username);
            return user;
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


        public void GetAlertHogerLager()
        {
            initNonExistingRepo();
            dataMgr = new DataManager();

            List<HogerLager> hogerLagers = repo.ReadHogerLagers().ToList();

            foreach(HogerLager hl in hogerLagers)
            {
                if(CalculateZscore(hl.Onderwerp) < CalculateZscore(hl.Onderwerp2))
                {
                    int i = 0;
                }
            }
        }

        public double CalculateZscore(Onderwerp onderwerp)
        {
            initNonExistingRepo();
            int totaalTweets = 0;
            //totaalTweets = messages.Where(Message => Message.Politician == s).Count();
            bool test;
            List<Message> messages = dataMgr.ReadMessagesWithSubjMsgs().ToList();
            List<Message> ms = new List<Message>();
            List<int> tweetsPerDag = new List<int>();
            double gemiddelde;
            Persoon p = (Persoon)onderwerp;
            DateTime laatsteTweet = messages.OrderBy(m => m.Date).ToList().Last().Date;

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
                sb.Append(g.Voornaam + " " + g.Naam);
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
        private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

            return d1 == d2;
        }
        public void SendMail(List<Alert> alerts, string email, string body)
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
    }
}
