using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IP3_8IEN.BL.Domain.Globalization;
using IP3_8IEN.DAL;

namespace IP3_8IEN.BL
{
    class GlobalizationManager : IGlobalizationManager
    {
        private IGlobalizationRepository repo;

        #region Constructor
        public GlobalizationManager()
        {

        }
        #endregion

        #region Platformen
        public void ChangePlatform(GlobalizationPlatform platform)
        {
            throw new NotImplementedException();
        }

        public GlobalizationPlatform CreatePlatform(string naam, string taal)
        {
            throw new NotImplementedException();
        }

        public GlobalizationPlatform GetPlatform(int id)
        {
            throw new NotImplementedException();
        }

        public GlobalizationPlatform GetPlatform(string naam, string taal)
        {
            throw new NotImplementedException();
        }

        public ICollection<GlobalizationPlatform> GetPlatformen()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Items
        public void ChangeItem(GlobalizationObject item)
        {
            throw new NotImplementedException();
        }

        public GlobalizationObject CreateItem(GlobalizationObject item)
        {
            throw new NotImplementedException();
        }

        public GlobalizationObject GetItemFromPlatform(GlobalizationPlatform platform, string key)
        {
            throw new NotImplementedException();
        }

        public ICollection<GlobalizationObject> GetItemsFromPlatform(GlobalizationPlatform platform)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void InitNonExistingRepo(bool withUOW)
        {
            //aangezien we normaal gezien hier normaal geen UOW
            //gaan moeten gebruiken, laten we de controle daarop
            //vallen en gaan we rechtstreeks op de repo zelf checken
            if (repo == null)
            {
                repo = new GlobalizationRepository()
            }
        }
    }
}
