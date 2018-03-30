using System.Collections.Generic;
using IP3_8IEN.BL.Domain.Data;

namespace IP3_8IEN.BL.Domain.Gebruikers
{
    public class AlertInstelling
    {
      
        //PK

        public int AlertInstellingId { get; private set; }
        public int GebruikerId { get; private set; }
        public int OnderwerpId { get; private set; }
        public int TresholdValue { get; private set; }
        public bool AlertAan { get; private set; }

        public Onderwerp Onderwerp { get; private set; }
        public Gebruiker Gebruiker { get; set; }
        public ICollection<Alert> Alerts { get; private set; }

    }
}