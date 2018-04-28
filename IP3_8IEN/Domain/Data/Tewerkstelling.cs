namespace IP_8IEN.BL.Domain.Data
{
    public class Tewerkstelling
    {
        public int TewerkstellingId { get; set; }

        public string Position { get; set; }

        public Persoon Persoon { get; set; }
        public Organisatie Organisatie { get; set; }
    }
}
