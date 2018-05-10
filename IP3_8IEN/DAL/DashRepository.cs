using IP_8IEN.BL.Domain.Dashboard;
using IP_8IEN.BL.Domain.Gebruikers;
using IP_8IEN.DAL.EF;
using IP3_8IEN.BL.Domain.Dashboard;
using System.Linq;

namespace IP_8IEN.DAL
{
    public class DashRepository : IDashRepository
    {
        private OurDbContext ctx;
        public bool isUoW;

        public DashRepository()
        {
            ctx = new OurDbContext();
            ctx.Database.Initialize(false);
        }

        public DashRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
        }

        public Dashbord ReadDashbord(Gebruiker user)
        {
            Dashbord dashbord = ctx.Dashbords.Include("TileZones").Include("User").FirstOrDefault(u => u.User.GebruikerId == user.GebruikerId);
            return dashbord;
        }

        public void AddFollow(Follow follow)
        {
            ctx.Follows.Add(follow);
            ctx.SaveChanges();
        }
        public void UpdateDashItem(DashItem dashItem)
        {
            ctx.Entry(dashItem).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void AddGraph(GraphData graph)
        {
            ctx.Graphs.Add(graph);
            ctx.SaveChanges();
        }

        public void AddDashBord(Dashbord dashbord)
        {
            ctx.Dashbords.Add(dashbord);
            ctx.SaveChanges();
        }

        ////UoW related
        //public DashRepository(UnitOfWork uow)
        //{
        //    ctx = uow.Context;
        //}
        public bool isUnitofWork()
        {
            return isUoW;
        }
        public void setUnitofWork(bool UoW)
        {
            isUoW = UoW;
        }

        //gebruik deze methode voor het type: 'Vergelijking','Kruising' en 'Cijfer'
        public void AddDashItem(DashItem dashItem)
        {
            ctx.DashItems.Add(dashItem);
        }
    }
}
