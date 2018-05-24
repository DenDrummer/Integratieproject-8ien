using IP3_8IEN.BL.Domain.Dashboard;
using IP3_8IEN.BL.Domain.Data;
using IP3_8IEN.BL.Domain.Gebruikers;
using IP3_8IEN.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IP3_8IEN.BL
{
    public class DashManager : IDashManager
    {
        private DataManager dataMgr;
        private GebruikerManager gebruikerMgr;
        private UnitOfWorkManager uowManager;
        private DashRepository repo;

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

        public Dashbord GetDashboard(int dashId)
        {
            initNonExistingRepo();

            Dashbord dash = repo.ReadDashbord(dashId);
            return dash;
        }

        public Dashbord GetDashboardWithFollows(Gebruiker user)
        {
            initNonExistingRepo();

            Dashbord dash = repo.ReadDashbordWithFollows(user);
            return dash;
        }

        public DashItem CreateDashitem(bool adminGraph)
        {
            initNonExistingRepo();

            DashItem dashItem;
            dashItem = new DashItem() { LastModified = System.DateTime.Now };

            if (adminGraph)
            {
                dashItem.AdminGraph = true;
            }
            else
            {
                dashItem.AdminGraph = false;
            }

            repo.AddDashItem(dashItem);

            return dashItem;
        }

        public Follow CreateFollow(int dashId, int id)
        {
            initNonExistingRepo(true);

            DashItem dashItem = repo.ReadDashItem(dashId);

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

        public List<Follow> CreateFollow(int dashId, List<int> listPersoonId)
        {
            initNonExistingRepo(true);

            DashItem dashItem = repo.ReadDashItem(dashId);
            List<Follow> follows = new List<Follow>();

            dataMgr = new DataManager(uowManager);
            bool UoW = false;
            repo.setUnitofWork(UoW);

            IEnumerable<Persoon> personen = dataMgr.GetPersonen();
            List<Persoon> listPersonen = new List<Persoon>();

            foreach(int persoonId in listPersoonId)
            {
                Follow follow = new Follow()
                {
                    DashItem = dashItem,
                    Onderwerp = personen.FirstOrDefault(p => p.OnderwerpId == persoonId)
                };
                follows.Add(follow);
                repo.AddFollow(follow);
            }
            uowManager.Save();

            UoW = true;
            repo.setUnitofWork(UoW);

            return follows;
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

        public DashItem SetupDashItem(Gebruiker user, List<Follow> follows)
        {
            initNonExistingRepo(true);

            bool UoW = false;
            repo.setUnitofWork(UoW);

            Dashbord dashbord = GetDashboard(user);

            TileZone tile = new TileZone()
            {
                Dashbord = dashbord,
                DashItem = follows[0].DashItem
            };
            uowManager.Save();

            foreach (Follow follow in follows)
            {
                repo.AddTileZone(tile);
                follow.DashItem.TileZones.Add(tile);
            }

            uowManager.Save();
            UoW = true;
            repo.setUnitofWork(UoW);

            return follows[0].DashItem;
        }

        public void LinkGraphsToUser(List<GraphData> graphDataList, int dashId)
        {
            initNonExistingRepo();

            DashItem dashItem = repo.ReadDashItem(dashId);
            dashItem.Graphdata = new Collection<GraphData>();

            foreach (GraphData graph in graphDataList)
            {
                dashItem.Graphdata.Add(graph);
                graph.DashItem = dashItem;
                repo.UpdateGraphData(graph);
            }

            //UpdateDashItem(dashItem);
        }

        public void AddGraph(GraphData graph)
        {
            initNonExistingRepo();

            repo.AddGraph(graph);
        }

        

        public void UpdateDashItem(DashItem dashItem)
        {
            repo.UpdateDashItem(dashItem);
        }

        public IEnumerable<Follow> GetFollows(bool admin = false)
        {
            initNonExistingRepo();
            IEnumerable<Follow> follows = repo.ReadFollows();

            if (admin)
            {
                //enkel vaste grafieken teruggeven
                follows = follows.Where(a => a.DashItem.AdminGraph == true);
                return follows;
            }
            else { return follows; }
        }

        public struct GraphdataValues
        {
            public double[] values;
            public string[] labels;
        }

        public GraphdataValues getGraphData(int persoonId, int aantalDagen)
        {
            initNonExistingRepo();

            dataMgr = new DataManager();
            Persoon persoon = dataMgr.GetPersoon(persoonId);
            List<GraphData> graphs = dataMgr.GetTweetsPerDag(persoon, 10);

            GraphdataValues graphsVals = new GraphdataValues()
            {
                values = new double[aantalDagen+1],
                labels = new string[aantalDagen+1]
            };

            int i = 0;
            foreach(GraphData graph in graphs)
            {
                graphsVals.values[i] = graph.value;
                graphsVals.labels[i] = graph.label;
                i++;
            }

            return graphsVals;
        }

        public Dashbord UpdateDashboard(Dashbord dashbord)
        {
            initNonExistingRepo(true);
            dataMgr = new DataManager(uowManager);
            repo.setUnitofWork(false);

            DateTime timeNow = DateTime.Now;
            foreach (TileZone tileZone in dashbord.TileZones)
            {
                double hours = (timeNow - tileZone.DashItem.LastModified).TotalHours;
                if (hours > 3)
                {
                    int i = 0;
                    //deze array verwijst naar de personen in GraphData
                    int[] persoonId = { 0, 0, 0, 0, 0 };

                    foreach (Follow follow in tileZone.DashItem.Follows)
                    {
                        try
                        {
                            persoonId[i] = follow.Onderwerp.OnderwerpId;
                            i++;
                        }
                        catch { throw new Exception("Out of bounds"); }

                    }

                    GraphdataValues graphs = getGraphData(persoonId[0],10/*Hier moet aantalDagen uit DashItem*/);

                    int j = 0;
                    foreach (var graph in tileZone.DashItem.Graphdata)
                    {
                        graph.label = graphs.labels[j];
                        graph.value = graphs.values[j];
                        repo.UpdateGraphData(graph);
                        j++;
                    }
                    uowManager.Save();
                }
                //LastModified updaten
                tileZone.DashItem.LastModified = timeNow;
                repo.UpdateTileZone(tileZone);
                uowManager.Save();
            }
            repo.setUnitofWork(true);
            return dashbord;
        }



        public IEnumerable<DashItem> GetDashItems()
        {
            IEnumerable<DashItem> dashItems = repo.ReadDashItems();
            return dashItems;
        }

        public void AddTileZone(TileZone tile)
        {
            repo.AddTileZone(tile);
        }

        public Dashbord AddDashBord(string userId)
        {
            initNonExistingRepo(true);

            gebruikerMgr = new GebruikerManager(uowManager);
            bool UoW = false;
            repo.setUnitofWork(UoW);

            //De te associëren gebruiker wordt opgehaald
            Gebruiker gebruiker = gebruikerMgr.GetGebruikers().FirstOrDefault(u => u.GebruikerId == userId);

            Dashbord dashbord = new Dashbord
            {
                User = gebruiker,
                TileZones = new Collection<TileZone>()
            };
            repo.AddDashBord(dashbord);
            uowManager.Save();
            repo.setUnitofWork(true);

            return dashbord;
        }


        public Dashbord DashbordInitGraphs(int dashId)
        {
            initNonExistingRepo();

            Dashbord dashbord = repo.ReadDashbord(dashId);

            //We halen vaste grafieken op (AdminGraphs) en koppelen deze aan de 
            //nieuw aangemaakte dashboard van de nieuwe gebruiker
            IEnumerable<DashItem> dashItems = GetDashItems();
            dashItems = dashItems.Where(d => d.AdminGraph == true);

            if (dashbord.TileZones == null)
            {
                dashbord.TileZones = new Collection<TileZone>();
            }

            foreach (DashItem item in dashItems)
            {
                TileZone tile = new TileZone()
                {
                    DashItem = item,
                    Dashbord = dashbord
                };
                repo.AddTileZone(tile);
            }
            repo.UpdateDashboard(dashbord);
            return dashbord;
        }

        public void InitializeDashbordNewUsers(string userId)
        {
            initNonExistingRepo();
            //Dashbord aanmaken en associëren met user
            Dashbord dashbord = AddDashBord(userId);

            //Dashbord initialiseren met vaste grafieken
            DashbordInitGraphs(dashbord.DashbordId);
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