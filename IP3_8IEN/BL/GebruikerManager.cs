using IP3_8IEN.BL.Domain.Data;
using IP3_8IEN.BL.Domain.Gebruikers;
using Newtonsoft.Json;
using System.IO;

using IP3_8IEN.DAL;
using IP3_8IEN.DAL.EF;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using IP3_8IEN.BL;

namespace IP3_8IEN.BL
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
            int onderwerpId;
            int thresh;


            dataMgr = new DataManager(uowManager);
            //We laten de transactie eve denken dat we geen 'UoW' gebruiken zodat er niet
            //van repo gewisseld wordt bij het aanroepen van een nieuwe methode
            bool UoW = false;
            repo.setUnitofWork(UoW);

            IEnumerable<Onderwerp> onderwerpen = dataMgr.ReadOnderwerpen();

            foreach (var item in alertInstellingen.records)
            {
                user = item.Username;
                onderwerpId = item.OnderwerpId;
                thresh = item.Threshold;

                Gebruiker gebruiker = FindUser(user);
                Onderwerp onderwerp = onderwerpen.FirstOrDefault(x => x.OnderwerpId == onderwerpId);

                AlertInstelling alertInst = new AlertInstelling()
                {
                    ThresholdVal = thresh,
                    AlertState = true,
                    Gebruiker = gebruiker,
                    Onderwerp = onderwerp
                };
                repo.AddingAlertInstelling(alertInst);
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

            AlertInstelling alertInstelling = repo.ReadAlertInstelling(alertInstellingId);
            Alert alert = new Alert()
            {
                AlertContent = alertContent,
                AlertInstelling = alertInstelling,
                CreatedOn = DateTime.Now
            };
            //alert toevoegen aan de ICollection van 'AlertInstelling'
            var alertColl = alertInstelling.alerts;
            if (alertColl != null)
            {
                alertInstelling.alerts = alertColl.ToList();
            }
            else
            {
                alertInstelling.alerts = new Collection<Alert>();
            }

            alertInstelling.alerts.Add(alert);

            //eerst alert creëren zodat deze een PK toegewegen krijgt
            repo.AddingAlert(alert);
            //dan de AlertInstelling updaten met de nieuwe 'Alert'
            repo.UpdateAlertInstelling(alertInstelling);
        }
        
        // Alerts inlezen via json bestand
        public void AddAlerts(string filePath)
        {
            initNonExistingRepo();

            StreamReader r = new StreamReader(filePath);
            string json = r.ReadToEnd();
            List<Message> alertList = new List<Message>();

            dynamic alerts = JsonConvert.DeserializeObject(json);

            string alertContent;
            int alertInstellingId;

            foreach (var item in alerts.records)
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
    }
}
