namespace IP_8IEN.BL.Domain.Data
{
    class Sentiment
    {
        public int Polariteit { get; private set; }
        public int Objectiviteit { get; private set; }

        public Sentiment(int polariteit, int objectiviteit)
        {
            Polariteit = polariteit;
            Objectiviteit = objectiviteit;
        }
    }
}
