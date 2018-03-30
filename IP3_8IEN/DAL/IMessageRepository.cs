using System.Collections.Generic;

using IP3_8IEN.BL.Domain.Data;

namespace IP3_8IEN.DAL
{
    public interface IMessageRepository
    {
        //16 mrt 2018 : Stephane
        void AddingMessage(Message message);

        //25 mrt 2018 : Stephane
        void AddOnderwerp(Onderwerp onderwerp);
        void AddSubjectMsg(SubjectMessage subjMsg);
        IEnumerable<Persoon> ReadPersonen();

        //28 mrt 2018 : Stephane
        IEnumerable<Hashtag> ReadHashtags();

    }
}
