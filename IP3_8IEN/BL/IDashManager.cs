using IP3_8IEN.BL.Domain.Dashboard;
using IP3_8IEN.BL.Domain.Gebruikers;
using System.Collections.Generic;

namespace IP3_8IEN.BL
{
    public interface IDashManager
    {
        void InitNonExistingRepo(bool withUnitOfWork);
        
        Dashbord GetDashboard(Gebruiker user);
        //DashItem AddDashItem(Gebruiker user, Onderwerp onderwerp);
        void AddGraph(GraphData graph);
        //Dashbord AddDashBord(Gebruiker gebruiker); // <-- 21 mei comment
        
        void UpdateDashItem(DashItem dashItem);
        DashItem SetupDashItem(/*DashItem dashItem, */Gebruiker user, Follow follow);
        void LinkGraphsToUser(List<GraphData> graphDataList, int dashId /*DashItem dashItem*/);
        DashItem CreateDashitem(bool adminGraph, string type, string naam, string town = "Vlaanderen");
        Follow CreateFollow(int dashId, int onderwerpid);
        
        IEnumerable<Follow> GetFollows(bool admin = false);
        
        //IEnumerable<TileZone> GetTileZones(); // <-- nog niet compleet
        IEnumerable<DashItem> GetDashItems();
        void AddTileZone(TileZone tile);
        void InitializeDashbordNewUsers(/*Gebruiker user */string userId);
        
        //void AddDashItems(Dashbord dashbord);
        Dashbord GetDashboard(int dashId);
        Dashbord DashbordInitGraphs(int dashId);
        
        Dashbord GetDashboardWithFollows(Gebruiker user);
        Dashbord UpdateDashboard(Dashbord dashbord);

        //Sam 
        List<DataChart> ExtractGraphList(int id);
        void updateTilezonesOrder(int dashId, string zonesOrder);
        void AddOneZonesOrder(Dashbord dashbord);
        void DeleteOneZonesOrder(Dashbord dashbord);

        //23 mei 2018 : Stephane
        List<Follow> CreateFollow(int dashId, List<int> listPersoonId);
        DashItem SetupDashItem(Gebruiker user, List<Follow> follows);
        void RemoveDashItem(int id);
        IEnumerable<Dashbord> GetDashbords();

        void SyncWithAdmins(string userId, int dashItemId);
        IEnumerable<GraphData> GetMostFollowsList();
    }
}