using IP_8IEN.BL.Domain.Dashboard;
using IP_8IEN.BL.Domain.Data;
using IP_8IEN.BL.Domain.Gebruikers;
using IP3_8IEN.BL.Domain.Dashboard;
using System.Collections.Generic;

namespace IP_8IEN.BL
{
    public interface IDashManager
    {
        //4 apr 2018 : Stephane
        void initNonExistingRepo(bool withUnitOfWork);

        //10 mei 2018 : Stephane
        Dashbord GetDashboard(Gebruiker user);
        //DashItem AddDashItem(Gebruiker user, Onderwerp onderwerp);
        void AddGraph(GraphData graph);
        //Dashbord AddDashBord(Gebruiker gebruiker); // <-- 21 mei comment

        //11 mei 2018 : Stephane
        void UpdateDashItem(DashItem dashItem);
        DashItem SetupDashItem(/*DashItem dashItem, */Gebruiker user, Follow follow);
        void LinkGraphsToUser(List<GraphData> graphDataList, int dashId /*DashItem dashItem*/);
        DashItem CreateDashitem(bool adminGraph);
        Follow CreateFollow(int dashId, int onderwerpid);

        //12 mei 2018 : Stephane
        IEnumerable<Follow> GetFollows(bool admin = false);

        //15 mei 2018 : Stephane
        //IEnumerable<TileZone> GetTileZones(); // <-- nog niet compleet
        IEnumerable<DashItem> GetDashItems();
        void AddTileZone(TileZone tile);
        void InitializeDashbordNewUsers(/*Gebruiker user */string userId);

        //21 mei 2018 : Stephane
        //void AddDashItems(Dashbord dashbord);
        Dashbord GetDashboard(int dashId);
        Dashbord DashbordInitGraphs(int dashId);

        //22 mei 2018 : Stephane
        Dashbord GetDashboardWithFollows(Gebruiker user);
        Dashbord UpdateDashboard(Dashbord dashbord);
    }
}