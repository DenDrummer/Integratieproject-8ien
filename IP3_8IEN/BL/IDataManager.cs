using IP_8IEN.BL.Domain.Data;
using System.Collections.Generic;

namespace IP_8IEN.BL
{
    public interface IDataManager
    {
        //16 mrt 2018 : Stephane
        //void AddMessages(string sourceUrl);

        //25 mrt 2018 : Stephane
        //Persoon AddPersoon(string voornaam, string achternaam);
        void AddSubjectMessage(Message msg, Persoon persoon);

        //28 mrt 2018 : Stephane
        Hashtag AddHashtag(string hashtag);
        void AddSubjectMessage(Message msg, Hashtag hashtag);

        //30 mrt 2018 : Stephane
        IEnumerable<Onderwerp> ReadOnderwerpen();

        //31 mrt 2018 : Stephane
        void initNonExistingRepo(bool withUnitOfWork);

        //2 apr 2018 : Stephane
        void AddOrganisation(string naamOrganisatie);
        void AddOrganisations(string filePath);
        void AddTewerkstelling(string naam, string organisatieNaam);
        void AddTewerkstelling(Persoon persoon, Organisatie organisatie);

        //6 apr 2018 : Stephane
        void ApiRequestToJson();

        //16 apr 2018 : Stephane
        void AddMessages(string sourceUrl);
        Persoon AddPersoon(string naam);
    }
}
