using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Dashboard
{
    public class DataChart2
    {
        public DataChart2(string label, double value, int value2, int value3, int value4, int value5)
        {
            this.label = label;
            this.value = value;
            this.value2 = value2;
            this.value3 = value3;
            this.value4 = value4;
            this.value5 = value5;
        }

        public DataChart2()
        {
        }
        public string label { get; set; }
        public double value { get; set; }
        public int value2 { get; set; }
        public int value3 { get; set; }
        public int value4 { get; set; }
        public int value5 { get; set; }
    }
}
