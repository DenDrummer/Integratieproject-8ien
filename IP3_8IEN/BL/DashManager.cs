using IP3_8IEN.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL
{
    class DashManager : IDashManager
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
