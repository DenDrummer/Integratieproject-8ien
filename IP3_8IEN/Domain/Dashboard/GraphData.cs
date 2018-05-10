using IP_8IEN.BL.Domain.Dashboard;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP3_8IEN.BL.Domain.Dashboard
{
    public class GraphData
    {
        public GraphData(string label, int value)
        {
            this.label = label;
            this.value1 = value;
        }

        public GraphData()
        {
        }

        [Key]
        public int GraphDataId { get; set; }
        public string label { get; set; }
        public int value1 { get; set; }

        DashItem DashItem { get; set; }
    }
}
