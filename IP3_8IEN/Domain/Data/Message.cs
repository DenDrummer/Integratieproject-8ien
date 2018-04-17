using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Data
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

        //twee getallen tussen -1 en 1
        public double SentimentPos { get; set; }
        public double SentimentNeg { get; set; }

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
    }
}
