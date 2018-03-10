using System.Collections.Generic;

namespace IP_8IEN.BL.Domain
{
    class Organisatie : Onderwerp
    {
        public List<Persoon> personen { get; private set; }
    }
}
