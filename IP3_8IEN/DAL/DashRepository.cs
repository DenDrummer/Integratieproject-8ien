using IP3_8IEN.BL.Domain.Dashboard;
using IP3_8IEN.BL.Domain.Gebruikers;
using IP3_8IEN.DAL.EF;
using System.Collections.Generic;
using System.Linq;

namespace IP3_8IEN.DAL
{
    public class DashRepository : IDashRepository
    {
        private OurDbContext ctx;
        public bool isUoW;

        public DashRepository()
        {
            ctx = new OurDbContext();
            isUoW = false;
            ctx.Database.Initialize(false);
        }

        public DashRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
            isUoW = true;
        }

        public Dashbord ReadDashbord(Gebruiker user)
        {
            Dashbord dashbord = ctx.Dashbords.Include("TileZones").Include("TileZones.DashItem").FirstOrDefault(u => u.User.GebruikerId == user.GebruikerId);
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

        //gebruik deze methode voor het type: 'Vergelijking','Kruising' en 'Cijfer'
        public void AddDashItem(DashItem dashItem)
        {
            ctx.DashItems.Add(dashItem);
            ctx.SaveChanges();
        }

        public void AddTileZone(TileZone tileZone)
        {
            ctx.TileZones.Add(tileZone);
            ctx.SaveChanges();
        }

        public void UpdateGraphData(GraphData graph)
        {
            ctx.Entry(graph).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public DashItem ReadDashItem(int dashId)
        {
            DashItem dashItem = ctx.DashItems.Find(dashId);
            return dashItem;
        }

        public void UpdateFollow(Follow follow)
        {
            ctx.Entry(follow).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public IEnumerable<Follow> ReadFollows()
        {
            IEnumerable<Follow> follows = ctx.Follows.Include("DashItem").Include("Onderwerp").ToList();
            return follows;
        }

        public void UpdateDashboard(Dashbord dashbord)
        {
            ctx.Entry(dashbord).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public IEnumerable<TileZone> ReadTileZones()
        {
            IEnumerable<TileZone> tileZones = ctx.TileZones;
            return tileZones;
        }

        public IEnumerable<DashItem> ReadDashItems()
        {
            IEnumerable<DashItem> dashItems = ctx.DashItems.ToList();
            return dashItems;
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
    }
}