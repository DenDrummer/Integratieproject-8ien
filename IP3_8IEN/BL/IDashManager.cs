using IP3_8IEN.BL.Domain.Dashboard;
using IP3_8IEN.BL.Domain.Gebruikers;
using System.Collections.Generic;

namespace IP3_8IEN.BL
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
        DashItem CreateDashitem(bool adminGraph, string type, string naam);
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

        //Sam 
        DashItem GetDashItemWithGraph(int id);
        List<GraphData> ExtractGraphList(int id);

        //23 mei 2018 : Stephane
        List<Follow> CreateFollow(int dashId, List<int> listPersoonId);
        DashItem SetupDashItem(Gebruiker user, List<Follow> follows);
        void RemoveDashItem(int id);
    }
}