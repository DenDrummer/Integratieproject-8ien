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
            //isUoW = false;
            ctx.Database.Initialize(false);
        }

        public DashRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
            //isUoW = true;
        }

        public Dashbord ReadDashbord(Gebruiker user)
            => ctx.Dashbords
            .Include("TileZones")
            .Include("TileZones.DashItem")
            .FirstOrDefault(u => u.User.GebruikerId == user.GebruikerId);

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
            => ctx.DashItems.Find(dashId);

        public void UpdateFollow(Follow follow)
        {
            ctx.Entry(follow).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public IEnumerable<Follow> ReadFollows()
            => ctx.Follows
            .Include("DashItem")
            .Include("Onderwerp")
            .ToList();

        public void UpdateDashboard(Dashbord dashbord)
        {
            ctx.Entry(dashbord).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public IEnumerable<TileZone> ReadTileZones()
            => ctx.TileZones;

        public IEnumerable<DashItem> ReadDashItems()
            => ctx.DashItems.ToList();

        public Dashbord ReadDashbord(int dashId)
            => ctx.Dashbords
            .Include("User")
            .FirstOrDefault(d => d.DashbordId == dashId);

        public Dashbord ReadDashbordWithFollows(Gebruiker user)
            => ctx.Dashbords
            .Include("TileZones")
            .Include("TileZones.DashItem")
            .Include("TileZones.DashItem.Graphdata")
            .Include("TileZones.DashItem.Follows")
            .Include("TileZones.DashItem.Follows.Onderwerp")
            .FirstOrDefault(u => u.User.GebruikerId == user.GebruikerId);

        public DashItem ReadDashItemWithGraph(int id)
        {
            return ctx.DashItems.Include("Graphdata").FirstOrDefault(d => d.DashItemId == id);
        }

        public void UpdateTileZone(TileZone tileZone)
        {
            ctx.Entry(tileZone).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        ////UoW related
        //public DashRepository(UnitOfWork uow)
        //{
        //    ctx = uow.Context;
        //}

        public bool IsUnitofWork()
            => isUoW;
        public void SetUnitofWork(bool UoW)
            => isUoW = UoW;
    }
}