using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using IP_8IEN.BL.Domain.Dashboard;

namespace IP_8IEN.BL.Domain.Gebruikers
{
    public class Gebruiker
    {
        [Key]
        public string GebruikerId { get; set; }
        public string Username { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Email { get; set; }
        public DateTime? Geboortedatum { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }

        public ICollection<WeeklyReview> WeeklyReviews { get; set; }
        public ICollection<Dashbord> Dashboards { get; set; }
        public ICollection<AlertInstelling> AlertInstellingen { get; set; }
    }
}