using IP_8IEN.BL.Domain.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP_8IEN.BL.Domain.Gebruikers
{
    public class AlertInstelling
    {
        [Key]
        public int AlertInstellingId { get; set; }
        public int ThresholdVal { get; set; }
        public bool AlertState { get; set; }

        public Gebruiker Gebruiker { get; set; }
        public Onderwerp Onderwerp { get; set; }

        public ICollection<Alert> alerts { get; set; }
    }
}
