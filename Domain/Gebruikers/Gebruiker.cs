using System;
using System.Collections.Generic;

using IP_8IEN.BL.Domain.Dash;

namespace IP_8IEN.BL.Domain.Gebruiker
{
    class Gebruiker
    {
        //PK
        public int gebruikerId { get; set; }
        public string naam { get; set; }
        public string achternaam { get; set; }
        public string email { get; set; }
        public DateTime geboortedatum { get; set; }

        //security implications
        public string password { get; set; }

        public ICollection<AlertInstelling> alertInstellingen { get; set; }
        public ICollection<Dashboard> dashboards { get; set; }
    }
}
