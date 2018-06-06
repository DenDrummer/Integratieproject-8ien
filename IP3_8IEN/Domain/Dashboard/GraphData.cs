using IP3_8IEN.BL.Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL.Domain.Dashboard
{
    public class GraphData
    {
        public GraphData(string label, double value)
        {
            this.Label = label;
            this.Value = value;
        }
        public GraphData(string label, int value, int value2, int value3, int value4, int value5)
        {
            this.Label = label;
            Value = value;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
            Value5 = value5;
        }

        public GraphData()
        {
        }

        [Key]
        public int GraphDataId { get; set; }
        public string Label { get; set; }
        public double Value { get; set; }
        public int Value2 { get; set; }
        public int Value3 { get; set; }
        public int Value4 { get; set; }
        public int Value5 { get; set; }

        public DashItem DashItem { get; set; }
    }
}
