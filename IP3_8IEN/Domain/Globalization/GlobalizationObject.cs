using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL.Domain.Globalization
{
    public class GlobalizationObject
    {
        [Key]
        public int Id;
        public GlobalizationPlatform Platform { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
