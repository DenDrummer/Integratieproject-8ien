using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL.Domain.Gebruikers
{
    public class Alert
    {
        public int AlertId { get; set; }
        public int VolgId { get; private set; }
        public AlertInstelling AlertInstelling;

        public Alert(int alertId, int volgId)
        {
            AlertId = alertId;
            VolgId = volgId;
        }

        public Alert()
        {
        }
    }
}
