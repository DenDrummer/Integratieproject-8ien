using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL.Domain.Dashboard
{
    public class DataChart
    {
        public DataChart(string label, double value)
        {
            this.Label = label;
            this.Value = value;
        }

        public DataChart()
        {
        }
        public string Label { get; set; }
        public double Value { get; set; }
    }
}
