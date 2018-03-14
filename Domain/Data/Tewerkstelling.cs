namespace IP_8IEN.BL.Domain.Data
{
    class Tewerkstelling
    {
        public int TewerkstellingId { get; private set; }
        public Organisatie Organisatie { get; private set; }
        public Persoon Persoon { get; private set; }
    }
}
