using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Data
{
    class Tewerkstelling
    {
        public int tewerkstellingId { get; set; }
        public Organisatie organisatie { get; set; }
        public Persoon persoon { get; set; }
    }
}
