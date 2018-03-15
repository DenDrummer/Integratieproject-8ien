using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IP_8IEN.BL.Domain.Data;
using IP_8IEN.BL.Domain.Gebruikers;
using IP_8IEN.DAL;

namespace IP_8IEN.BL
{
    public class GebruikerManager
    {
        //        private UnitOfWorkManager uowManager;
        private GebruikerRepository GebruikersRepo;
        private AlertRepository AlertRepo;


        public GebruikerManager()
        {
            GebruikersRepo = new GebruikerRepository();
            AlertRepo = new AlertRepository();
        }

        //        public GebruikerManager(UnitOfWorkManager uowMgr)
        //        {
        //            uowManager = uowMgr;
        //            repo = new GebruikerRepository();
        //        }

        public IEnumerable<Gebruiker> GetGebruikers()
        {
            return GebruikersRepo.ReadGebruikers();
        }

        public IEnumerable<Alert> GetAlerts()
        {
            return AlertRepo.ReadAlerts();
        }


        //        public void initNonExistingRepo(bool withUnitOfWork = false)
        //        {
        //            // Als we een repo met UoW willen gebruiken en als er nog geen uowManager bestaat:
        //            // Dan maken we de uowManager aan en gebruiken we de context daaruit om de repo aan te maken.
        //
        //            if (withUnitOfWork)
        //            {
        //                if (uowManager == null)
        //                {
        //                    uowManager = new UnitOfWorkManager();
        //                    repo = new GebruikerRepository(uowManager.UnitOfWork);
        //                }
        //            }
        //            // Als we niet met UoW willen werken, dan maken we een repo aan als die nog niet bestaat.
        //            else
        //            {
        //                repo = (repo == null) ? new DAL.GebruikerRepository() : repo;
        //            }
        //        }
    }
}