using IP_8IEN.BL.Domain.Data;
using IP3_8IEN.BL.Domain.Dashboard;
using System.ComponentModel.DataAnnotations;

namespace IP_8IEN.BL.Domain.Dashboard
{
    public class Follow
    {
        [Key]
        public int FollowId { get; set; }

        GraphData Graph { get; set; }
        GraphData2 Graph2 { get; set; }

        public DashItem DashItem { get; set; }
        public Onderwerp Onderwerp { get; set; }
    }
}