using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP_8IEN.BL.Domain.Dashboard
{
    public class Deelplatform
    {
        [Key]
        public int DeelplatformId { get; set; }
        public string databron { get; set; }

        public ICollection<Dashbord> Dashboards { get; set; }
    }
}
