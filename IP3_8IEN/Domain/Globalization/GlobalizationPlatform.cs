using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP3_8IEN.BL.Domain.Globalization
{
    public class GlobalizationPlatform
    {
        [Key]
        public int PlatformId { get; set; }
        public string Platform { get; set; } = "Default";
        public string Language { get; set; } = "NL";
        public ICollection<GlobalizationObject> Items { get; set; }
        //if the key does not exist, it will loop through these until it finds the key (it will not use the fallback of the fallback)
        public ICollection<KeyValuePair<int, GlobalizationPlatform>> FallBackPlatformen { get; set; }
    }
}
