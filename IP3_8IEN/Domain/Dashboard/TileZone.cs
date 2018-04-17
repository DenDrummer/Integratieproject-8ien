using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Dashboard
{
    public class TileZone
    {
        [Key]
        public int TileZoneId { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public double positionX { get; set; }
        public double positionY { get; set; }

        public Dashbord Dashbord { get; set; }
        public DashItem DashItem { get; set; }
    }
}
