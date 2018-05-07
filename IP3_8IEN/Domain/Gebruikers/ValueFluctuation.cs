using IP_8IEN.BL.Domain.Gebruikers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL.Domain.Gebruikers
{
    public class ValueFluctuation : AlertInstelling
    {
        public int ThresholdValue { get; set; }
    }
}
