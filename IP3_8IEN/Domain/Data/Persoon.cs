using System;
using System.Collections.Generic;

namespace IP_8IEN.BL.Domain.Data
{
    public class Persoon : Onderwerp
    {
        //Naam bevat voor- en achternaam
        public string Naam { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public DateTime Geboortedatum { get; set; }
        public string District { get; set; }


        //onderstaande 5 items staan nergens in de view
        public string Level { get; set; }
        public string Gender { get; set; }
        public string Site { get; set; }
        public int PostalCode { get; set; }
        public string Town { get; set; }



        public ICollection<Tewerkstelling> Tewerkstellingen { get; set; }
    }
}
