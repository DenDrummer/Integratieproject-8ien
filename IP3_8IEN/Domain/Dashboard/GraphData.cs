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

        public GraphData()
        {
        }

        [Key]
        public int GraphDataId { get; set; }
        public string Label { get; set; }
        public double Value { get; set; }

        public DashItem DashItem { get; set; }
    }
}
