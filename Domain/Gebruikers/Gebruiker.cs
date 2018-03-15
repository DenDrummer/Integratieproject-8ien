using IP_8IEN.BL.Domain.Dash;
using System;
using System.Collections.Generic;

namespace IP_8IEN.BL.Domain.Gebruikers
{
    public class Gebruiker
    {
        //PK
        public int GebruikerId { get; set; }
     
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        //[RegularExpression("([a-z0-9.]*)@([a-z.]*)")]
        public string Email { get; set; }
        public DateTime GeboorteDatum { get; set; }

        #region security implications
        public string Password { get; set; }
        #endregion

        public ICollection<AlertInstelling> AlertInstellingen { get; private set; }
        public ICollection<Dashboard> Dashboards { get; private set; }

        public Gebruiker(int gebruikerId, string email)
        {
            GebruikerId = gebruikerId;
            Email = email;
        }

        public Gebruiker()
        {
        }
    }
}
