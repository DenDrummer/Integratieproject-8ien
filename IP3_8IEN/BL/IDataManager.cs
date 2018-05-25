using IP3_8IEN.BL.Domain.Data;
using System;
using System.Collections.Generic;
using IP3_8IEN.BL.Domain.Dashboard;
using IP3_8IEN.BL.Domain.Gebruikers;

namespace IP3_8IEN.BL
{
    public interface IDataManager
    {
        //void AddMessages(string sourceUrl);
        
        SubjectMessage AddSubjectMessage(Message msg, Persoon persoon);
        
        Hashtag AddHashtag(string hashtag);
        void AddSubjectMessage(Message msg, Hashtag hashtag);
        
        IEnumerable<Onderwerp> ReadOnderwerpen();
        
        void initNonExistingRepo(bool withUnitOfWork);
        
        Organisatie AddOrganisation(string naamOrganisatie);
        void AddOrganisations(string filePath);
        void AddTewerkstelling(string naam, string organisatieNaam);
        
        void ApiRequestToJson();
        
        void AddMessages(string json);
        Persoon AddPersoon(string naam);

        
        void AddPersonen(string path);
        void AddTewerkstelling(Persoon persoon, string organisatie);
        int CountSubjMsgsPersoon(Onderwerp onderwerp); //Onderwerp onderwerp

        
        IEnumerable<Message> ReadMessagesWithSubjMsgs();
        
        void GetAlerts();
        
        void SendMail();
        List<GraphData> GetRanking(int aantal, int interval_uren, bool puntNotatie = true);
        int GetNumber(Persoon persoon, int laatsteAantalUren = 0);
        List<GraphData> GetTweetsPerDag(Persoon persoon, int aantalDagenTerug = 0);
        List<GraphData> GetTweetsPerDagList(Persoon persoon, int aantalDagenTerug = 0);

        List<GraphData2> GetTweetsPerDag2(Persoon persoon1, Persoon persoon2, Persoon persoon3, Persoon persoon4,
            Persoon persoon5, int aantalDagenTerug = 0);
        
        Persoon GetPersoon(int persoonId);
        Organisatie GetOrganisatie(int organisatieId);
        
        IEnumerable<Persoon> GetPersonen();
        IEnumerable<Organisatie> GetOrganisaties();
        
        void ChangeOrganisation(Organisatie organisatie);
        
        string GetImageString(string screenname);
        string GetBannerString(string screenname);
        
        List<int> ExtractListPersoonId(IEnumerable<GraphData> graphDataList);
        Persoon GetPersoonWithTewerkstelling(string naam);
        Persoon GetPersoonWithTewerkstelling(int id);
        
        double GetPolarityByPerson(Persoon persoon);
        double GetPolarityByPerson(Persoon persoon, DateTime start);
        double GetPolarityByPerson(Persoon persoon, DateTime start, DateTime stop);
        double GetObjectivityByPerson(Persoon persoon);
        double GetObjectivityByPerson(Persoon persoon, DateTime start);
        double GetObjectivityByPerson(Persoon persoon, DateTime start, DateTime stop);
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

        #region Organisaties
        #endregion

        #region Personen
        void ChangePersoon(Persoon persoon);
        Persoon GetPersoon(string naam);
        #endregion

        #region Themas
        #endregion
    }
}