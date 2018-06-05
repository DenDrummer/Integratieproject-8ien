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
        #region platformen
        ICollection<GlobalizationPlatform> ReadPlatformen();
        GlobalizationPlatform ReadPlatform(int id);
        GlobalizationPlatform ReadPlatform(string name, string language);
        void AddPlatform(GlobalizationPlatform platform);
        void UpdatePlatform(GlobalizationPlatform platform);
        #endregion

        #region items
        ICollection<GlobalizationObject> ReadItemsFromPlatform(GlobalizationPlatform platform);
        GlobalizationObject ReadItemFromPlatform(GlobalizationPlatform platform, string key);
        void AddItem(GlobalizationObject item);
        void UpdateItem(GlobalizationObject item);
        #endregion
    }
}
