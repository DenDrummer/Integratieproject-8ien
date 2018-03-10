using System.Collections.Generic;

using IP_8IEN.BL.Domain.Dash;

namespace IP_8IEN.BL.Domain.Data
{
    class Message
    {
        //PK
        public int messageId { get; set; }
        public string source { get; set; }
        //id van oorspronkelijke tweet, misschien niet nodig
        public int id { get; set; }
        //user id van oorspronkelijke tweet
        public int user_id { get; set; }
        //geo format nog onbekend
        public string geo { get; set; }
        public List<string> mentions { get; set; }
        public bool retweet { get; set; }
        public System.DateTime date { get; set; }
        public List<string> words { get; set; }
        //twee getallen tussent -1 en 1
        public int sentiment { get; set; }

        public ICollection<SubjectMessage> subjectmessages { get; set; }
    }
}
