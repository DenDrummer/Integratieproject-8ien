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

        public DashItem AddDashItem(Gebruiker user, Onderwerp onderwerp)
        {
            initNonExistingRepo();

            DashItem dashItem = new DashItem();
            repo.AddDashItem(dashItem);

            Follow follow = new Follow()
            {
                Onderwerp = onderwerp,
                DashItem = dashItem
            };
            repo.AddFollow(follow);
            
            ICollection<Follow> follows = new Collection<Follow>();
            follows.Add(follow);
            dashItem.Follows = follows;

            Dashbord dashbord = GetDashboard(user);
            TileZone tile = new TileZone()
            {
                Dashbord = dashbord,
                DashItem = dashItem
            };

            dashItem.TileZones.Add(tile);
            repo.UpdateDashItem(dashItem);

            return dashItem;
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
