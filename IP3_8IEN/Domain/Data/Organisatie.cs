using System.Collections.Generic;

namespace IP_8IEN.BL.Domain.Data
{
    public class Organisatie : Onderwerp
    {
        public string NaamOrganisatie { get; set; }

        public ICollection<Tewerkstelling> Tewerkstellingen { get; set; }
    }
}
