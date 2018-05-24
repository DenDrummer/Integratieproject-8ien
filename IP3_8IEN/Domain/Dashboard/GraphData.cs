using IP_8IEN.BL.Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL.Domain.Dashboard
{
    public class GraphData
    {
        public GraphData(string label, double value)
        {
            this.label = label;
            this.value = value;
        }

        public GraphData()
        {
        }

        [Key]
        public int GraphDataId { get; set; }
        public string label { get; set; }
        public double value { get; set; }

        public DashItem DashItem { get; set; }
    }
}
