namespace IP3_8IEN.BL.Domain.Dashboard
{
    public class DataChart
    {
        public DataChart(string label, double value)
        {
            Label = label;
            Value = value;
        }

        public DataChart()
        {
        }
        public string Label { get; set; }
        public double Value { get; set; }
    }
}
