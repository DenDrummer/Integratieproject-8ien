using IP3_8IEN.BL.Domain.Gebruikers;

namespace IP3_8IEN.BL.Domain.Gebruikers
{
    public class ValueFluctuation : AlertInstelling
    {
        public int ThresholdValue { get; set; }
        public int CurrentValue { get; set; }
    }
}
