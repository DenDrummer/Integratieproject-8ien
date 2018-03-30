using IP3_8IEN.BL.Domain.Data;

namespace IP3_8IEN.BL
{
    public interface IDataManager
    {
        //16 mrt 2018 : Stephane
        void AddMessages(string sourceUrl);

        //25 mrt 2018 : Stephane
        Persoon AddPersoon(string voornaam, string achternaam);
        void AddSubjectMessage(Message msg, Persoon persoon);

        //28 mrt 2018 : Stephane
        Hashtag AddHashtag (string hashtag);
        void AddSubjectMessage(Message msg, Hashtag hashtag);

    }
}
