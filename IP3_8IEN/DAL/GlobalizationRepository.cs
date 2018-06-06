using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IP3_8IEN.BL.Domain.Globalization;
using IP3_8IEN.DAL;
using IP3_8IEN.DAL.EF;

namespace IP3_8IEN.DAL
{
    public class GlobalizationRepository : IGlobalizationRepository
    {
        private OurDbContext ctx;

        #region Constructors
        public GlobalizationRepository()
        {
            ctx = new OurDbContext();
        }

        public GlobalizationRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
        }
        #endregion

        #region Platformen
        //List
        public ICollection<GlobalizationPlatform> ReadPlatformen()
            => ctx.GlobalizationPlatforms
            //.Include("Items")
            //.Include("FallBackPlatformen")
            .ToList();

        //Create
        public void AddPlatform(GlobalizationPlatform platform)
        {
            ctx.GlobalizationPlatforms.Add(platform);
            ctx.SaveChanges();
        }

        //Read
        public GlobalizationPlatform ReadPlatform(int id)
            => ctx.GlobalizationPlatforms.Find(id);

        public GlobalizationPlatform ReadPlatform(string name, string language)
            => ctx.GlobalizationPlatforms
                .Include("Items")
                .Include("FallBackPlatformen")
                .ToList()
                .FirstOrDefault(p => p.Platform.Equals(name) && p.Language.Equals(language));

        //Update
        public void UpdatePlatform(GlobalizationPlatform platform)
        {
            ctx.Entry(platform).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        //Delete
        public void DeletePlatform(GlobalizationPlatform platform)
        {
            foreach (GlobalizationObject item in platform.Items)
            {
                ctx.Entry(item).State = EntityState.Deleted;
            }
            ctx.Entry(platform).State = EntityState.Deleted;
            ctx.SaveChanges();
        }
        #endregion

        #region Items
        //List
        public ICollection<GlobalizationObject> ReadItemsFromPlatform(GlobalizationPlatform platform)
            => ctx.GlobalizationItems
            .Where(i => i.Platform.Equals(platform))
            .ToList();

        //Create
        public void AddItem(GlobalizationObject item)
        {
            ctx.GlobalizationItems.Add(item);
            ctx.SaveChanges();
        }

        //Read
        public GlobalizationObject ReadItem(int itemId)
            => ctx.GlobalizationItems.Find(itemId);

        public GlobalizationObject ReadItemFromPlatform(GlobalizationPlatform platform, string key)
            => ctx.GlobalizationItems
                .Where(i => i.Platform.Equals(platform))
                .ToList()
                .FirstOrDefault(i => i.Key.Equals(key));

        //Update
        public void UpdateItem(GlobalizationObject item)
        {
            ctx.Entry(item).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        //Delete
        public void DeleteItem(GlobalizationObject item)
        {
            ctx.Entry(item).State = EntityState.Deleted;
            ctx.SaveChanges();
        }
        #endregion
    }
}
