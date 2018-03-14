using IP_8IEN.BL.Domain.Dash;
using IP_8IEN.BL.Domain.Gebruikers;
using IP_8IEN.BL.Domain.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP_8IEN.BL.Domain.Data
{
    class Onderwerp
    {
        //PK
        public int OnderwerpId { get; private set; }
        public string Naam { get; private set; }
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorMaxChar")]
        public string Beschrijving { get; private set; }
        public string GekoppeldeTermen { get; private set; }

        //public Image Foto { get; private set; }

        public ICollection<SubjectMessage> SubjectMessages { get; private set; }
        public ICollection<Follow> Follows { get; private set; }
        public ICollection<AlertInstelling> AlertInstellingen { get; private set; }
    }
}
