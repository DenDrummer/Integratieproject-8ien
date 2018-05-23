using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Dashboard
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
