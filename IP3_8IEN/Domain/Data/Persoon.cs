using IP_8IEN.BL.Domain.Dashboard;
using IP_8IEN.BL.Domain.Gebruikers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Data
{
    public class Persoon : Onderwerp
    {
        //Naam bevat voor- en achternaam
        public string Naam { get; set; }
        public string District { get; set; }
        public string Level { get; set; }
        public string Gender { get; set; }
        public string Twitter { get; set; }
        public string Site { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Facebook { get; set; }
        public int PostalCode { get; set; }
        public string Town { get; set; }


        public ICollection<Tewerkstelling> Tewerkstellingen { get; set; }
    }
}
