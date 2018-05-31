using IP3_8IEN.BL.Domain.Data;
using System;
using System.Collections.Generic;
using IP3_8IEN.BL.Domain.Dashboard;

namespace IP3_8IEN.BL
{
    public interface IDataManager
    {
        #region Alerts
        void GetAlerts();
        #endregion

        #region Onderwerpen
        IEnumerable<Onderwerp> ReadOnderwerpen();

        #region Organisaties
        Organisatie GetOrganisatie(int organisatieId);
        IEnumerable<Organisatie> GetOrganisaties();
        void ChangeOrganisation(Organisatie organisatie);
        Organisatie AddOrganisation(string naamOrganisatie);
        void AddOrganisations(string filePath);
        #endregion

        #region Personen
        void ChangePersoon(Persoon persoon);
        Persoon GetPersoon(string naam);
        Persoon GetPersoon(int persoonId);
        IEnumerable<Persoon> GetPersonen();
        //6 apr 2018 : Stephane
        void ApiRequestToJson(bool isReCheck = false);
        string ExportToCSV(IEnumerable<Persoon> personen);

        //16 apr 2018 : Stephane
        void AddMessages(string json);
        Persoon AddPersoon(string naam);
        void AddPersonen(string path);
        Persoon GetPersoonWithTewerkstelling(string naam);
        Persoon GetPersoonWithTewerkstelling(int id);
        List<int> ExtractListPersoonId(IEnumerable<GraphData> graphDataList);
        double GetPolarityByPerson(Persoon persoon);
        double GetPolarityByPerson(Persoon persoon, DateTime start);
        double GetPolarityByPerson(Persoon persoon, DateTime start, DateTime stop);
        double GetObjectivityByPerson(Persoon persoon);
        double GetObjectivityByPerson(Persoon persoon, DateTime start);
        double GetObjectivityByPerson(Persoon persoon, DateTime start, DateTime stop);
        int CountSubjMsgsPersoon(Onderwerp onderwerp); //Onderwerp onderwerp
        #endregion

        #region Themas
        //WIP in de ThemaMethodes branch op de github
        #endregion
        #endregion

        #region (Subject)Messages
        IEnumerable<Message> ReadMessagesWithSubjMsgs();
        void AddSubjectMessage(Message msg, Hashtag hashtag);
        SubjectMessage AddSubjectMessage(Message msg, Persoon persoon);
        //void AddMessages(string sourceUrl);
        #endregion

        #region Tewerkstellingen
        void AddTewerkstelling(Persoon persoon, string organisatie);
        void AddTewerkstelling(string naam, string organisatieNaam);
        #endregion

        IEnumerable<Hashtag> GetHashtags();

        #region Unsorted
        int GetMentionCountByName(string naam);
        int GetMentionCountByName(string naam, DateTime start);
        int GetMentionCountByName(string naam, DateTime start, DateTime stop);
        List<GraphData> GetTopWordsCount();
        List<GraphData> GetTopWordsCount(int aantal);
        List<GraphData> GetTopWordsCount(int aantal, DateTime start);
        List<GraphData> GetTopWordsCount(int aantal, DateTime start, DateTime stop);
        List<GraphData> GetTopWordsCount(DateTime start, DateTime stop);
        int GetWordCountByName(string name);
        int GetWordCountByName(string name, DateTime start);
        int GetWordCountByName(string name, DateTime start, DateTime stop);
        List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2);
        List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2, Persoon p3);
        List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2, Persoon p3, Persoon p4);
        List<GraphData> GetComparisonPersonNumberOfTweets(Persoon p1, Persoon p2, Persoon p3, Persoon p4, Persoon p5);
        List<GraphData> GetTopStoryCount();
        List<GraphData> GetTopStoryCount(int aantal);
        List<GraphData> GetTopStoryCount(int aantal, DateTime start);
        List<GraphData> GetTopStoryCount(int aantal, DateTime start, DateTime stop);
        List<GraphData> GetTopStoryCount(DateTime start, DateTime stop);
        List<GraphData> GetTopStoryByPolitician(Persoon persoon);
        List<GraphData2> GetComparisonPersonNumberOfTweetsOverTime(Persoon p1, Persoon p2, Persoon p3, Persoon p4, Persoon p5);
        List<GraphData> GetTopMentions(int aantal);
        void SendMail();
        List<GraphData> GetRanking(int aantal, int interval_uren, bool puntNotatie = true);
        int GetNumber(Persoon persoon, int laatsteAantalUren = 0);
        List<GraphData> GetTweetsPerDag(Persoon persoon, int aantalDagenTerug = 0);
        List<GraphData> GetTweetsPerDagList(Persoon persoon, int aantalDagenTerug = 0);
        List<GraphData2> GetTweetsPerDag2(Persoon persoon1, Persoon persoon2, Persoon persoon3, Persoon persoon4,
            Persoon persoon5, int aantalDagenTerug = 0);
        string GetImageString(string screenname);
        string GetBannerString(string screenname);
        Hashtag AddHashtag(string hashtag);
        void InitNonExistingRepo(bool withUnitOfWork);
        #endregion
    }
}