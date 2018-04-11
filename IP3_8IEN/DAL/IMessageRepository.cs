using System.Collections.Generic;

using IP_8IEN.BL.Domain.Data;

namespace IP_8IEN.DAL
{
    public interface IMessageRepository
    {
        //Dit is de repo voor de 'Data' package

        //16 mrt 2018 : Stephane
        void AddingMessage(Message message);

        //25 mrt 2018 : Stephane
        void AddOnderwerp(Onderwerp onderwerp);
        void AddSubjectMsg(SubjectMessage subjMsg);
        IEnumerable<Persoon> ReadPersonen();

        //28 mrt 2018 : Stephane
        IEnumerable<Hashtag> ReadHashtags();

        //30 mrt 2018 : Stephane
        IEnumerable<Onderwerp> ReadSubjects();

        //1 apr 2018 : Stephane
        bool isUnitofWork();
        void setUnitofWork(bool UoW);

        //2 apr 2018 : Stephane
        IEnumerable<Organisatie> ReadOrganisaties();
        void AddingTewerkstelling(Tewerkstelling tewerkstelling);
        void UdateOnderwerp(Onderwerp onderwerp);
    }
}
