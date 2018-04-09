using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using IP3_8IEN.BL.Domain.Gebruikers;
using IP3_8IEN.BL.Domain.Dashboard;


namespace IP3_8IEN.BL.Domain.Data
{

        public abstract class Onderwerp
        {
            //PK
            [Key]
            public int OnderwerpId { get; set; }
            [MaxLength(100)]
            public string Beschrijving { get; set; }
            public string Twitter { get; set; }
            
            //public Image Foto { get; private set; }

            public ICollection<SubjectMessage> SubjectMessages { get; set; }
            public ICollection<Follow> Follows { get; set; }
            public ICollection<AlertInstelling> AlertInstellingen { get; set; }
        }
}
