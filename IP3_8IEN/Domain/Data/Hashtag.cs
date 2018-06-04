namespace IP3_8IEN.BL.Domain.Data
{
    public class Hashtag : Onderwerp
    {
        //public string HashtagString { get; set; }
        public bool Thema { get; set; }

        //Deze wordt niet opgeslagen, enkel voor bewerkingen
        public int Vermelding { get; set; }
    }
}
