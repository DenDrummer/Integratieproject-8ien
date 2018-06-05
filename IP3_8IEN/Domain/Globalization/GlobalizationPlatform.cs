using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Globalization
{
    public class GlobalizationPlatform
    {
        public ICollection<GlobalizationObject> Items { get; set; }
        public ICollection<GlobalizationPlatform> FallBackPlatformen { get; set; }
    }
}
