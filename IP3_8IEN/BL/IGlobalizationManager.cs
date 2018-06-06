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
        //List
        IEnumerable<GlobalizationPlatform> GetPlatformen();

        //Create
        GlobalizationPlatform CreatePlatform(string naam, string taal);

        //Read
        GlobalizationPlatform GetPlatform(int id);
        GlobalizationPlatform GetPlatform(string naam, string taal);

        //Update
        void ChangePlatform(GlobalizationPlatform platform);

        //Delete
        void DeletePlatform(GlobalizationPlatform platform);
        #endregion

        #region Items
        //List
        IEnumerable<GlobalizationObject> GetItemsFromPlatform(int platformId);

        //Create
        GlobalizationString CreateItem(int platformId, string key, string value);
        //GlobalizationImage CreateItem(int platformId, string key, Image value);
        //GlobalizationObject CreateItem(int platformId, string key, object value);

        //Read
        GlobalizationString GetItem(int itemId);
        GlobalizationString GetItemFromPlatform(int platformId, string key);

        //Update
        void ChangeItem(GlobalizationString item);

        //Delete
        void DeleteItem(GlobalizationString item);
        #endregion
    }
}
