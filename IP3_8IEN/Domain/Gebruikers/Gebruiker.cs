using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IP3_8IEN.BL.Domain.Dashboard;

namespace IP3_8IEN.BL.Domain.Gebruikers
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
        public ICollection<Dash> Dashboards { get; private set; }

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
