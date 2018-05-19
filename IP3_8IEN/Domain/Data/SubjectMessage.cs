using System.ComponentModel.DataAnnotations;

namespace IP_8IEN.BL.Domain.Data
{
    public class SubjectMessage
    {
        [Key]
        public int SubjectMsgId { get; set; }
        public Message Msg { get; set; }
        public Persoon Persoon { get; set; }
        public Hashtag Hashtag { get; set; }
        public Organisatie Organisatie { get; set; }
    }
}