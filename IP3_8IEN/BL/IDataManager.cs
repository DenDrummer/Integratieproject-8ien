using IP_8IEN.BL.Domain.Data;
using System;
using System.Collections.Generic;

namespace IP_8IEN.BL
{
    public interface IDataManager
    {
        //16 mrt 2018 : Stephane
        //void AddMessages(string sourceUrl);

        //25 mrt 2018 : Stephane
        //Persoon AddPersoon(string voornaam, string achternaam);
        SubjectMessage AddSubjectMessage(Message msg, Persoon persoon);  //20 apr 2018 Victor (update: subjmes toevoegen aan tweet)

        //28 mrt 2018 : Stephane
        Hashtag AddHashtag(string hashtag);
        void AddSubjectMessage(Message msg, Hashtag hashtag);

        //30 mrt 2018 : Stephane
        IEnumerable<Onderwerp> ReadOnderwerpen();

        //31 mrt 2018 : Stephane
        void initNonExistingRepo(bool withUnitOfWork);

        //2 apr 2018 : Stephane
        Organisatie AddOrganisation(string naamOrganisatie);
        void AddOrganisations(string filePath);
        void AddTewerkstelling(string naam, string organisatieNaam);

        //6 apr 2018 : Stephane
        void ApiRequestToJson();

        //16 apr 2018 : Stephane
        void AddMessages(string json);
        Persoon AddPersoon(string naam);


        //22 apr 2018 : Stephane
        void AddPersonen(string path);
        void AddTewerkstelling(Persoon persoon, string organisatie);
        int CountSubjMsgsPersoon(Onderwerp onderwerp); //Onderwerp onderwerp


        //23 apr 2018 : Stephane
        IEnumerable<Onderwerp> ReadOnderwerpenWithSubjMsgs();
        IEnumerable<Message> ReadMessagesWithSubjMsgs();

        //24 apr 2018 : Victor
        void GetAlerts();

        //27 apr 2018 : Victor
        void SendMail();
        Dictionary<Persoon, double> GetRanking(int aantal, int interval_uren, bool puntNotatie = true);
        double CalculateChange(long previous, long current);
        int GetNumber(Persoon persoon, int laatsteAantalUren = 0);
        Dictionary<DateTime, int> GetTweetsPerDag(Persoon persoon, int aantalDagenTerug = 0);


    }
}
