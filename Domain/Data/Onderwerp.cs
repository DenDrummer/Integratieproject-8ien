using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using IP_8IEN.BL.Domain.Dash;
using IP_8IEN.BL.Domain.Gebruiker;
using IP_8IEN.BL.Domain.Properties;

namespace IP_8IEN.BL.Domain.Data
{
    class Onderwerp
    {
        //PK
        public int onderwerpId { get; set; }
        public string naam { get; set; }
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorMaxChar")]
        public string beschrijving { get; set; }
        public string gekoppeldeTermen { get; set; }

        //public ImageSource Image foto {get;set;}

        public ICollection<SubjectMessage> subjectmessages { get; set; }
        public ICollection<Follow> follows { get; set; }
        public ICollection<AlertInstelling> alertInstellingen { get; set; }
    }
}
