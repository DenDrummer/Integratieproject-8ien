using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IP_8IEN.BL.Domain.Globalization;
using IP3_8IEN.DAL;
using IP3_8IEN.DAL.EF;

namespace IP_8IEN.DAL
{
    class GlobalizationRepository : IGlobalizationRepository
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
        public void AddPlatform(GlobalizationPlatform platform)
        {
            ctx.GlobalizationPlatforms.Add(platform);
            ctx.SaveChanges();
        }

        public GlobalizationPlatform ReadPlatform(int id)
            => ctx.GlobalizationPlatforms.Find(id);

        public GlobalizationPlatform ReadPlatform(string name, string language)
            => ctx.GlobalizationPlatforms
                .Include("Items")
                .Include("FallBackPlatformen")
                .ToList()
                .FirstOrDefault(p => p.Name.Equals(name) && p.Language.Equals(language));

        public void UpdatePlatform(GlobalizationPlatform platform)
        {
            ctx.Entry(platform).State = EntityState.Modified;
            ctx.SaveChanges();
        }
        #endregion

        #region Items
        public void AddItem(GlobalizationObject item)
        {
            ctx.GlobalizationItems.Add(item);
            ctx.SaveChanges();
        }

        public ICollection<GlobalizationObject> ReadItemsFromPlatform(GlobalizationPlatform platform)
        {

        }

        public GlobalizationObject ReadItemFromPlatform(GlobalizationPlatform platform, string key)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(GlobalizationObject item)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
