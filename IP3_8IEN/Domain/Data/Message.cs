using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL.Domain.Data
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string Source { get; set; }
        //id van oorspronkelijke tweet, misschien niet nodig
        public long Id { get; set; }
        //user id van oorspronkelijke tweet .. Voorlopig een string >> "N/A"
        public string UserId { get; set; }
        //geo format nog onbekend
        public string Geo { get; set; }
        public bool Retweet { get; set; }
        public DateTime Date { get; set; }

        //(key)Words in message
        public string Word1 { get; set; }
        public string Word2 { get; set; }
        public string Word3 { get; set; }
        public string Word4 { get; set; }
        public string Word5 { get; set; }

        //twee getallen tussen -1 en 1
        //nog te wijzigen naar double
        public int SentimentPos { get; set; }
        public int SentimentNeg { get; set; }

        //urls
        public string Url1 { get; set; }
        public string Url2 { get; set; }

        //public ICollection<string> Mentions { get; set; }
        public string Mention1 { get; set; }
        public string Mention2 { get; set; }
        public string Mention3 { get; set; }
        public string Mention4 { get; set; }
        public string Mention5 { get; set; }

        public ICollection<SubjectMessage> SubjectMessages { get; set; }
    }
}
