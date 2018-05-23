using IP3_8IEN.BL.Domain.Data;
using System.ComponentModel.DataAnnotations;

namespace IP3_8IEN.BL.Domain.Dashboard
{
    public class Follow
    {
        [Key]
        public int FollowId { get; set; }

        public DashItem DashItem { get; set; }
        public Onderwerp Onderwerp { get; set; }
    }
}