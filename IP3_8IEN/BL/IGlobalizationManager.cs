using IP3_8IEN.BL.Domain.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace IP3_8IEN.BL
{
    public interface IGlobalizationManager
    {
        void InitNonExistingRepo(bool withUOW);

        #region Platformen
        GlobalizationPlatform CreatePlatform(string naam, string taal);
        GlobalizationPlatform GetPlatform(int id);
        GlobalizationPlatform GetPlatform(string naam, string taal);
        IEnumerable<GlobalizationPlatform> GetPlatformen();
        void ChangePlatform(GlobalizationPlatform platform);
        #endregion

        #region Items
        IEnumerable<GlobalizationObject> GetItemsFromPlatform(int platformId);
        GlobalizationObject GetItemFromPlatform(int platformId, string key);
        GlobalizationString CreateItem(int platformId, string key, string value);
        //GlobalizationImage CreateItem(int platformId, string key, Image value);
        GlobalizationObject CreateItem(int platformId, string key, object value);
        void ChangeItem(GlobalizationObject item);
        #endregion
    }
}
