using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Dashboard
{
    public class DataChart
    {
        public DataChart(string label, double value)
        {
            this.label = label;
            this.value = value;
        }

        public DataChart()
        {
        }
        public string label { get; set; }
        public double value { get; set; }
    }
}
