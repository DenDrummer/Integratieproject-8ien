using IP3_8IEN.BL.Domain.Dashboard;
using IP3_8IEN.BL.Domain.Data;
using IP3_8IEN.BL.Domain.Gebruikers;
using System.Collections.Generic;

namespace IP3_8IEN.DAL
{
    public interface IDashRepository
    {
        bool IsUnitofWork();
        void SetUnitofWork(bool UoW);
        //void AddDashItem(DashItem dashItem);
        
        Dashbord ReadDashbord(Gebruiker user);
        void AddFollow(Follow follow);
        void UpdateDashItem(DashItem dashItem);
        void AddGraph(GraphData graph);
        void AddDashBord(Dashbord dashbord);
        
        void AddTileZone(TileZone tileZone);
        void UpdateGraphData(GraphData graph);
        DashItem ReadDashItem(int dashId);
        void UpdateFollow(Follow follow);
        
        IEnumerable<Follow> ReadFollows();
        void UpdateDashboard(Dashbord dashbord);
        
        IEnumerable<TileZone> ReadTileZones(); // nog niet compleet
        IEnumerable<DashItem> ReadDashItems();
        
        Dashbord ReadDashbord(int dashId);
        
        Dashbord ReadDashbordWithFollows(Gebruiker user);
        Dashbord ReadDashbordWithFollows(int dashId);
        void UpdateTileZone(TileZone tileZone);
        DashItem ReadDashItemWithGraph(int id);
       
        //Sam
        //DashItem ReadDashItemWithGraph(int dashId);
        //Dashbord ReadDashbordWithFollows(int dashId);
        Dashbord ReadDefaultDashbord();
    }
}