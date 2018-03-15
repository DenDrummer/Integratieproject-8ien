using System.Collections.Generic;

namespace IP_8IEN.BL.Domain.Data
{
    class Organisatie : Onderwerp
    {
        public string TwitterUrl { get; private set; }
        //public DateTime OprichtingsDatum { get; private set; }

        public ICollection<Tewerkstelling> Tewerkstellingen { get; private set; }
    }
}
