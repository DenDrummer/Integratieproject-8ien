using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IP_8IEN.BL.Domain.Data;

namespace IP_8IEN.BL.Domain.Gebruiker
{
    class AlertInstelling
    {
        //PK
        public int alertConfigId { get; set; }

        public Gebruiker gebruiker { get; set; }
        public Onderwerp onderwerp { get; set; }

        public int treshholdValue { get; set; }
        public bool alert_aan { get; set; }

        public ICollection<Alert> alerts { get; set; }
    }
}
