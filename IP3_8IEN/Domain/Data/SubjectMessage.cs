using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL.Domain.Data
{
    public class SubjectMessage
    {
        [Key]
        public int SubjectMsgId { get; set; }
        public Message Msg { get; set; }
        public Persoon Persoon { get; set; }
        public Hashtag Hashtag { get; set; }
    }
}
