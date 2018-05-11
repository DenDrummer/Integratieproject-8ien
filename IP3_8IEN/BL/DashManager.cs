using IP_8IEN.BL.Domain.Dashboard;
using IP_8IEN.BL.Domain.Data;
using IP_8IEN.BL.Domain.Gebruikers;
using IP_8IEN.DAL;
using IP3_8IEN.BL.Domain.Dashboard;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IP_8IEN.BL
{
    public class DashManager : IDashManager
    {
        private DataManager dataMgr;
        private UnitOfWorkManager uowManager;
        private DashRepository repo;//= new MessageRepository();

        // Deze constructor gebruiken we voor operaties binnen de package
        public DashManager()
        {
            
        }
        // We roepen deze constructor aan wanneer we met twee repositories gaan werken
        public DashManager(UnitOfWorkManager uowMgr)
        {
            uowManager = uowMgr;
            repo = new DashRepository(uowManager.UnitOfWork);
        }

        public Dashbord GetDashboard(Gebruiker user)
        {
            initNonExistingRepo();

            Dashbord dash = repo.ReadDashbord(user);
            return dash;
        }

        public DashItem CreateDashitem()
        {
            initNonExistingRepo();

            DashItem dashItem;
            dashItem = new DashItem();
            repo.AddDashItem(dashItem);

            return dashItem;
        }

        public Follow CreateFollow(int dashId, int id)
        {
            initNonExistingRepo(true);

            DashItem dashItem = repo.GetDashItem(dashId);

            dataMgr = new DataManager(uowManager);
            bool UoW = false;
            repo.setUnitofWork(UoW);

            Persoon onderwerp = dataMgr.GetPersoon(id);

            Follow follow = new Follow()
            {
                DashItem = dashItem,
                Onderwerp = onderwerp
            };
            repo.AddFollow(follow);

            uowManager.Save();

            UoW = true;
            repo.setUnitofWork(UoW);

            return follow;
        }

        public DashItem SetupDashItem(/*DashItem dashItem,*/ Gebruiker user, Follow follow)
        {
            initNonExistingRepo(true);

            bool UoW = false;
            repo.setUnitofWork(UoW);

            follow.DashItem.Follows = new Collection<Follow>();
            follow.DashItem.Follows.Add(follow);
            
            Dashbord dashbord = GetDashboard(user);

            TileZone tile = new TileZone()
            {
                Dashbord = dashbord,
                DashItem = follow.DashItem
            };

            repo.AddTileZone(tile);
            follow.DashItem.TileZones.Add(tile);
            repo.UpdateFollow(follow);
            //repo.UpdateDashItem(dashItem);

            uowManager.Save();
            UoW = true;
            repo.setUnitofWork(UoW);

            return follow.DashItem;
        }

        public void LinkGraphsToUser(List<GraphData> graphDataList, int dashId /*DashItem dashItem*/)
        {
            initNonExistingRepo();

            //bool UoW = false;
            //repo.setUnitofWork(UoW);

            DashItem dashItem = repo.GetDashItem(dashId);
            dashItem.Graphdata = new Collection<GraphData>();

            foreach (GraphData graph in graphDataList)
            {
                dashItem.Graphdata.Add(graph);
                graph.DashItem = dashItem;
                repo.UpdateGraphData(graph);
            }

            UpdateDashItem(dashItem);

            //uowManager.Save();
            //UoW = true;
            //repo.setUnitofWork(UoW);
        }

        public void AddGraph(GraphData graph)
        {
            initNonExistingRepo();

            repo.AddGraph(graph);
        }

        public Dashbord AddDashBord(Gebruiker gebruiker)
        {
            initNonExistingRepo();

            Dashbord dashbord = new Dashbord
            {
                User = gebruiker,
                TileZones = new Collection<TileZone>()
            };
            repo.AddDashBord(dashbord);
            return dashbord;
        }

        public void UpdateDashItem(DashItem dashItem)
        {
            repo.UpdateDashItem(dashItem);
        }

        //Unit of Work related
        public void initNonExistingRepo(bool withUnitOfWork = false)
        {
            // Als we een repo met UoW willen gebruiken en als er nog geen uowManager bestaat:
            // Dan maken we de uowManager aan en gebruiken we de context daaruit om de repo aan te maken.
            if (withUnitOfWork)
            {
                if (uowManager == null)
                {
                    uowManager = new UnitOfWorkManager();
                }
                repo = new DAL.DashRepository(uowManager.UnitOfWork);
            }
            // Als we niet met UoW willen werken, dan maken we een repo aan als die nog niet bestaat.
            else
            {
                //zien of repo al bestaat
                if (repo == null)
                {
                    repo = new DAL.DashRepository();
                }
                else
                {
                    //checken wat voor repo we hebben
                    bool isUoW = repo.isUnitofWork();
                    if (isUoW)
                    {
                        repo = new DAL.DashRepository();
                    }
                    else
                    {
                        // repo behoudt zijn context
                    }
                }
            }
        }
    }
}
