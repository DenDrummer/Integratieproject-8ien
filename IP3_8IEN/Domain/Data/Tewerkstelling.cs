using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL.Domain.Data
{
    public class Tewerkstelling
    {
        public int TewerkstellingId { get; set; }

        public Persoon Persoon { get; set; }
        public Organisatie Organisatie { get; set; }
    }
}
