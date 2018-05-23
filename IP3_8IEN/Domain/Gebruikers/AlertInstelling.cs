using IP3_8IEN.BL.Domain.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP3_8IEN.BL.Domain.Gebruikers
{
    public abstract class AlertInstelling
    {
        [Key]
        public int AlertInstellingId { get; set; }
        public bool AlertState { get; set; }
        public bool NotificationWeb { get; set; }
        public bool Email { get; set; }
        public bool MobileNotification { get; set; }
        public string Type { get; set; }

        public Gebruiker Gebruiker { get; set; }
        public Onderwerp Onderwerp { get; set; }


        public ICollection<Alert> Alerts { get; set; }
    }
}
