using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Data
{
    class SubjectMessage
    {
        public int subjMessageId { get; set; }
        public Message message { get; set; }
        public Onderwerp onderwerp { get; set; }
    }
}
