using System.Collections.Generic;

namespace IP3_8IEN.BL.Domain.Data
{
    public class Thema : Onderwerp
    {
        public string ThemaString { get; set; }
        public ICollection<string> Hashtags { get; set; }
    }
}
