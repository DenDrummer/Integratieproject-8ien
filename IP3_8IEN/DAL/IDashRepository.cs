using IP_8IEN.BL.Domain.Dashboard;
using IP_8IEN.BL.Domain.Data;
using IP_8IEN.BL.Domain.Gebruikers;
using System.Collections.Generic;

namespace IP_8IEN.DAL
{
    public interface IDashRepository
    {
        //5 apr 2018
        bool isUnitofWork();
        void setUnitofWork(bool UoW);
        //void AddDashItem(DashItem dashItem);

        //10 mei 2018 : Stephane
        Dashbord ReadDashbord(Gebruiker user);
        void AddFollow(Follow follow);
        void UpdateDashItem(DashItem dashItem);
        void AddGraph(GraphData graph);
        void AddDashBord(Dashbord dashbord);

        //11 mei 2018 : Stephane
        void AddTileZone(TileZone tileZone);
        void UpdateGraphData(GraphData graph);
        DashItem ReadDashItem(int dashId);
        void UpdateFollow(Follow follow);

        //12 mei 2018 : Stephane
        IEnumerable<Follow> ReadFollows();
        void UpdateDashboard(Dashbord dashbord);

        //15 mei 2018 : Stephane
        IEnumerable<TileZone> ReadTileZones(); // <-- nog niet compleet
        IEnumerable<DashItem> ReadDashItems();

        //21 mei 2018 : Stephane
        Dashbord ReadDashbord(int dashId);

        //22 mei 2018 : Stephane
        Dashbord ReadDashbordWithFollows(Gebruiker user);
        void UpdateTileZone(TileZone tileZone);
        //Sam
        DashItem ReadDashItemWithGraph(int dashId);
        Dashbord ReadDashbordWithFollows(int dashId);
    }
}