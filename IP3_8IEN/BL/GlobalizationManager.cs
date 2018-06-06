using System;
using System.Collections.Generic;
using System.Drawing;
using IP3_8IEN.BL.Domain.Globalization;
using IP3_8IEN.DAL;

namespace IP3_8IEN.BL
{
    public class GlobalizationManager : IGlobalizationManager
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
            InitNonExistingRepo();
            repo.UpdatePlatform(platform);
        }

        public GlobalizationPlatform CreatePlatform(string naam, string taal)
        {
            InitNonExistingRepo();

            GlobalizationPlatform platform = new GlobalizationPlatform()
            {
                Platform = naam,
                Language = taal,
                Items = new List<GlobalizationObject>(),
                FallBackPlatformen = new List<KeyValuePair<int, GlobalizationPlatform>>()
            };
            repo.AddPlatform(platform);

            return platform;
        }

        public GlobalizationPlatform GetPlatform(int id)
        {
            InitNonExistingRepo();
            GlobalizationPlatform platform = repo.ReadPlatform(id);
            return platform;
        }

        public GlobalizationPlatform GetPlatform(string naam, string taal)
        {
            InitNonExistingRepo();
            GlobalizationPlatform platform = repo.ReadPlatform(naam, taal);
            return platform;
        }

        public IEnumerable<GlobalizationPlatform> GetPlatformen()
        {
            InitNonExistingRepo();
            IEnumerable<GlobalizationPlatform> platformen = repo.ReadPlatformen();
            return platformen;
        }
        #endregion

        #region Items
        public void ChangeItem(GlobalizationObject item)
        {
            InitNonExistingRepo();
            repo.UpdateItem(item);
        }

        public GlobalizationString CreateItem(int platformId, string key, string value)
        {
            InitNonExistingRepo();
            GlobalizationPlatform platform = GetPlatform(platformId);
            GlobalizationString item = new GlobalizationString()
            {
                Platform = platform,
                Key = key,
                Value = value
            };
            repo.UpdatePlatform(platform);
            repo.AddItem(item);
            return item;
        }

        /*public GlobalizationImage CreateItem(int platformId, string key, Image value)
        {
            InitNonExistingRepo();
            GlobalizationPlatform platform = GetPlatform(platformId);
            GlobalizationImage item = new GlobalizationImage()
            {
                Platform = platform,
                Key = key,
                Value = value
            };
            repo.UpdatePlatform(platform);
            repo.AddItem(item);
            return item;
        }*/

        public GlobalizationObject CreateItem(int platformId, string key, object value)
        {
            InitNonExistingRepo();
            GlobalizationPlatform platform = GetPlatform(platformId);
            GlobalizationObject item = new GlobalizationObject()
            {
                Platform = platform,
                Key = key,
                Value = value
            };
            repo.UpdatePlatform(platform);
            repo.AddItem(item);
            return item;
        }

        public GlobalizationObject GetItemFromPlatform(int platformId, string key)
        {
            InitNonExistingRepo();
            GlobalizationPlatform platform = GetPlatform(platformId);
            GlobalizationObject item = repo.ReadItemFromPlatform(platform, key);
            if (item == null && platform.FallBackPlatformen != null)
            {
                for (int i = 0; i < platform.FallBackPlatformen.Count; i++)
                {
                }
                foreach (KeyValuePair<int, GlobalizationPlatform> kvp in platform.FallBackPlatformen)
                {
                    item = repo.ReadItemFromPlatform(kvp.Value, key);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }
            return item;
        }

        public IEnumerable<GlobalizationObject> GetItemsFromPlatform(int platformId)
        {
            InitNonExistingRepo();
            GlobalizationPlatform platform = GetPlatform(platformId);
            IEnumerable<GlobalizationObject> items = repo.ReadItemsFromPlatform(platform);
            return items;
        }
        #endregion

        public void InitNonExistingRepo(bool withUOW = false)
        {
            //aangezien we normaal gezien hier normaal geen UOW
            //gaan moeten gebruiken, laten we de controle daarop
            //vallen en gaan we rechtstreeks op de repo zelf checken
            if (repo == null)
            {
                repo = new GlobalizationRepository();
            }
            //else: repo behoudt zijn context
        }
    }
}
