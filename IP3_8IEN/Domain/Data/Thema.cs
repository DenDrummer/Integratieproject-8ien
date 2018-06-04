using IP3_8IEN.BL.Domain.Dashboard;
using IP3_8IEN.BL.Domain.Gebruikers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP3_8IEN.BL.Domain.Data
{
    public class Thema : Onderwerp
    {
        //[Key]
        //public int OnderwerpId { get; set; }
        //[MaxLength(100)]
        //public string Beschrijving { get; set; }
        
        //public string Naam { get; set; }

        public string Hashtag1 { get; set; }
        public string Hashtag2 { get; set; }
        public string Hashtag3 { get; set; }
        public string Hashtag4 { get; set; }

        public ICollection<string> Hashtags { get; set; }

        //public ICollection<SubjectMessage> SubjectMessages { get; set; }
        //public ICollection<Follow> Follows { get; set; }
        //public ICollection<AlertInstelling> AlertInstellingen { get; set; }
    }
}
