using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IP_8IEN.BL.Domain.Dashboard;

namespace IP_8IEN.BL.Domain.Gebruikers
{
    public class Gebruiker
    {
        [Key]
        public int GebruikerId { get; set; }
        public string Username { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Email { get; set; }
        public DateTime Geboortedatum { get; set; }

        //enum
        public string Role { get; set; }

        //public string Psswd { get; set; }

        public ICollection<WeeklyReview> WeeklyReviews { get; set; }
        public ICollection<Dashbord> Dashboards { get; set; }
        public ICollection<AlertInstelling> AlertInstellingen { get; set; }
    }
}
