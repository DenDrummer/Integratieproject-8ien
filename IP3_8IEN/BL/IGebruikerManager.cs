using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IP3_8IEN.BL.Domain.Gebruikers;

namespace IP3_8IEN.BL
{
    public interface IGebruikerManager
    {
        IEnumerable<Gebruiker> GetGebruikers();
        IEnumerable<Alert> GetAlerts();
        void AddMessage();

    }
}
