using IP_8IEN.BL.Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_S.Models
{
    public class GraphDataViewModel
    {
        public Dashbord dash { get; set; }
        public List<GraphData> GraphData { get; set; }
        public TileZone TileZone { get; set; }
    }
}