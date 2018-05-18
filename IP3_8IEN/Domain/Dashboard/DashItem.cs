using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP_8IEN.BL.Domain.Dashboard
{
    public abstract class DashItem
    {
        [Key]
        public int DashItemId { get; set; }

        public ICollection<TileZone> TileZones { get; set; }
        public ICollection<Follow> Follows { get; set; }
    }
}
