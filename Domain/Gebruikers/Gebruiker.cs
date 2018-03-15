using IP_8IEN.BL.Domain.Dash;
using System;
using System.Collections.Generic;

namespace IP_8IEN.BL.Domain.Gebruikers
{
    class Gebruiker
    {
        //PK
        public int GebruikerId { get; private set; }
        public string Voornaam { get; private set; }
        public string Achternaam { get; private set; }
        //[RegularExpression("([a-z0-9.]*)@([a-z.]*)")]
        public string Email { get; private set; }
        public DateTime GeboorteDatum { get; private set; }

        #region security implications
        public string Password { get; private set; }
        #endregion

        public ICollection<AlertInstelling> AlertInstellingen { get; private set; }
        public ICollection<Dashboard> Dashboards { get; private set; }
    }
}
