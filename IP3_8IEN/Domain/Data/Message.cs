using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP3_8IEN.BL.Domain.Data
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string Source { get; set; }
        //id van oorspronkelijke tweet
        public long Id { get; set; }

        //user id van oorspronkelijke tweet .. Voorlopig een string >> "N/A"
        //public string UserId { get; set; }

        public bool Retweet { get; set; }
        public DateTime Date { get; set; }

        //profile
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Education { get; set; }
        public string Language { get; set; }
        public string Personality { get; set; }

        //(key)Words in message
        public string Word1 { get; set; }
        public string Word2 { get; set; }
        public string Word3 { get; set; }
        public string Word4 { get; set; }
        public string Word5 { get; set; }

        //getal tussen -1 en 1
        public double Polarity { get; set; }
        //getal tussen 0 en 1
        public double Objectivity { get; set; }

        //urls
        public string Url1 { get; set; }
        public string Url2 { get; set; }

        //mentions
        public string Mention1 { get; set; }
        public string Mention2 { get; set; }
        public string Mention3 { get; set; }
        public string Mention4 { get; set; }
        public string Mention5 { get; set; }

        //geolocatie
        public double Geo1 { get; set; }
        public double Geo2 { get; set; }

        public ICollection<SubjectMessage> SubjectMessages { get; set; }

        public bool IsFromPersoon(Persoon persoon)
        {
            foreach (SubjectMessage s in SubjectMessages)
            {
                if (s.Persoon == persoon)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsFromOrganisatie(Organisatie organisatie)
        {
            foreach (SubjectMessage s in SubjectMessages)
            {
               
                foreach (Tewerkstelling t in s.Persoon.Tewerkstellingen)
                {
                    if (t.Organisatie == organisatie)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

       
    }
}
