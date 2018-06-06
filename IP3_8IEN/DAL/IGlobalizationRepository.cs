using IP3_8IEN.BL.Domain.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.DAL
{
    public interface IGlobalizationRepository
    {
        #region Platformen
        //List
        ICollection<GlobalizationPlatform> ReadPlatformen();
        //Create
        void AddPlatform(GlobalizationPlatform platform);
        //Read
        GlobalizationPlatform ReadPlatform(int id);
        GlobalizationPlatform ReadPlatform(string name, string language);
        //Update
        void UpdatePlatform(GlobalizationPlatform platform);
        //Delete
        void DeletePlatform(GlobalizationPlatform platform);
        #endregion

        #region Items
        //List
        ICollection<GlobalizationObject> ReadItemsFromPlatform(GlobalizationPlatform platform);
        //Create
        void AddItem(GlobalizationObject item);
        //Read
        GlobalizationObject ReadItem(int itemId);
        GlobalizationObject ReadItemFromPlatform(GlobalizationPlatform platform, string key);
        //Update
        void UpdateItem(GlobalizationObject item);
        //Delete
        void DeleteItem(GlobalizationObject item);
        #endregion
    }
}
