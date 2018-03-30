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

        //PK
        /*[Key]
        public int PersoonId { get; set; }
        [MaxLength(100)]
        public string Beschrijving { get; set; }
        public string Twitter { get; set; }

        //public Image Foto { get; private set; }

        public ICollection<SubjectMessage> SubjectMessages { get; set; }
        public ICollection<Follow> Follows { get; set; }
        public ICollection<AlertInstelling> AlertInstellingen { get; set; }*/

        public string Voornaam { get; set; }
        //Achternaam kan meerdere woorden bevatten vb: "van de .."
        public string Achternaam { get; set; }
    }
}
