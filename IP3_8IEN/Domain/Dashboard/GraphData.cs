using IP_8IEN.BL.Domain.Dashboard;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP_8IEN.BL.Domain.Dashboard
{
    public class GraphData
    {
        public GraphData(string label, int value)
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
        public int value { get; set; }

        public DashItem DashItem { get; set; }
    }
}