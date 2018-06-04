using System.Collections.Generic;

using IP3_8IEN.BL.Domain.Data;

namespace IP3_8IEN.DAL
{
    public interface IMessageRepository
    {
        //Dit is de repo voor de 'Data' package
        
        void AddingMessage(Message message);
        
        void AddOnderwerp(Onderwerp onderwerp);
        void AddSubjectMsg(SubjectMessage subjMsg);
        IEnumerable<Persoon> ReadPersonen();
        IEnumerable<Persoon> ReadPersonenOnly();


        IEnumerable<Hashtag> ReadHashtags();
        
        IEnumerable<Onderwerp> ReadSubjects();
        
        bool IsUnitofWork();
        void SetUnitofWork(bool UoW);
        
        IEnumerable<Organisatie> ReadOrganisaties();
        void AddingTewerkstelling(Tewerkstelling tewerkstelling);
        void UdateOnderwerp(Onderwerp onderwerp);
        
        IEnumerable<SubjectMessage> ReadSubjectMessages();
        void UpdateMessage();
        
        IEnumerable<Message> ReadMessages();
        
        Persoon ReadPersoon(int persoonId);
        IEnumerable<Tewerkstelling> ReadTewerkstellingen();
        
        IEnumerable<Message> ReadMessages(bool subjM);
        Organisatie ReadOrganisatie(int organisatieId);
        
        void EditOrganisation(Organisatie organisatie);
        void EditPersoon(Persoon persoon);

        Persoon ReadPersoon(string naam);
        Persoon ReadPersoonWithSbjctMsg(int persoonId);
        Persoon ReadPersoonWithTewerkstelling(string naam);
        Persoon ReadPersoonWithTewerkstelling(int id);

        void UpdateHashtag(Hashtag hashtag);
        void CreateTheme(Thema theme);
        IEnumerable<Thema> ReadThemas();
        void UpdateTheme(Thema theme);
        IEnumerable<Persoon> ReadPersonenWithTewerkstelling();
    }
}