using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using IP3_8IEN.BL.Domain.Gebruikers;

namespace IP3_8IEN.BL.Domain.Dashboard
{
    public class Dashbord
    {
        [Key]
        public int DashbordId { get; set; }

        public Deelplatform Deelplatform { get; set; }
        public Gebruiker Gebruiker { get; set; }
        public ICollection<TileZone> TileZones { get; set; }
    }
}
