using IP3_8IEN.BL.Domain.Dashboard;
using IP3_8IEN.BL.Domain.Gebruikers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL.Domain.Data
{
    public class Persoon : Onderwerp
    {
        //Achternaam kan meerdere woorden bevatten vb: "van de .."
        public string Achternaam { get; set; }
        public string Voornaam { get; set; }

        public ICollection<Tewerkstelling> Tewerkstellingen { get; set; }
    }
}
