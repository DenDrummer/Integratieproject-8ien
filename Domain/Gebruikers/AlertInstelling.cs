using IP_8IEN.BL.Domain.Data;
using System.Collections.Generic;

namespace IP_8IEN.BL.Domain.Gebruikers
{
    class AlertInstelling
    {
        //PK
        public int AlertInstellingId { get; private set; }
        public Gebruiker Gebruiker { get; private set; }
        public Onderwerp Onderwerp { get; private set; }
        public int TresholdValue { get; private set; }
        public bool AlertAan { get; private set; }

        public ICollection<Alert> alerts { get; private set; }
    }
}
