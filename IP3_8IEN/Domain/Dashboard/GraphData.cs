namespace IP3_8IEN.BL.Domain.Dashboard
{
    public class GraphData
    {
        public string label { get; set; }
        public int value { get; set; }

        public GraphData(string label, int value)
        {
            this.label = label;
            this.value = value;
        }

        public GraphData()
        {
        }
    }
}
