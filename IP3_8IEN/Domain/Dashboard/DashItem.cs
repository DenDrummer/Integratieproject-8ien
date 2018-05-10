﻿using IP3_8IEN.BL.Domain.Dashboard;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP_8IEN.BL.Domain.Dashboard
{
    public class DashItem
    {
        [Key]
        public int DashItemId { get; set; }

        public ICollection<GraphData> Graphdata { get; set; }
        public ICollection<GraphData2> Graphdata2 { get; set; }

        public ICollection<TileZone> TileZones { get; set; }
        public ICollection<Follow> Follows { get; set; }
    }
}
