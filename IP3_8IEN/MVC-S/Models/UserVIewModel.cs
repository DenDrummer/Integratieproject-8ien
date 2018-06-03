using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_S.Models
{
    public class UserVIewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Passw { get; set; }
        [Required]
        public string Role { get; set; }
        public string Username { get; set; }
        public string Voornaam { get; set; }
        public string Naam { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}