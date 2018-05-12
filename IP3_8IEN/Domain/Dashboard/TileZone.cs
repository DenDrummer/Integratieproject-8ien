using System.ComponentModel.DataAnnotations;

namespace IP_8IEN.BL.Domain.Dashboard
{
    public class TileZone
    {
        [Key]
        public int TileZoneId { get; set; }
        
        public Dashbord Dashbord { get; set; }
        public DashItem DashItem { get; set; }
    }
}
