using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Globalization
{
    public class GlobalizationPlatform
    {
        [Key]
        public int PlatformId { get; set; }
        public string Name { get; set; } = "Default";
        public string Language { get; set; } = "N";
        public ICollection<GlobalizationObject> Items { get; set; }
        public ICollection<GlobalizationPlatform> FallBackPlatformen { get; set; }
    }
}
