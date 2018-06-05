using IP3_8IEN.BL.Domain.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL
{
    interface IGlobalizationManager
    {
        void InitNonExistingRepo(bool withUOW);

        #region Platformen
        GlobalizationPlatform CreatePlatform(string naam, string taal);
        GlobalizationPlatform GetPlatform(int id);
        GlobalizationPlatform GetPlatform(string naam, string taal);
        ICollection<GlobalizationPlatform> GetPlatformen();
        void ChangePlatform(GlobalizationPlatform platform);
        #endregion

        #region Items
        ICollection<GlobalizationObject> GetItemsFromPlatform(GlobalizationPlatform platform);
        GlobalizationObject GetItemFromPlatform(GlobalizationPlatform platform, string key);
        GlobalizationObject CreateItem(GlobalizationObject item);
        void ChangeItem(GlobalizationObject item);
        #endregion
    }
}
