using System.Collections.Generic;

namespace IP_8IEN.BL.Domain.Data
{
    class Organisatie : Onderwerp
    {
        public string twitterurl { get; set; }
        //public DateTime oprichtingsdatum { get; set; }

        public ICollection<Tewerkstelling> tewerkstellingen { get; private set; }
    }
}
