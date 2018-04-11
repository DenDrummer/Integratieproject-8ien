using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IP_8IEN.BL.Domain.Dashboard;
using IP_8IEN.DAL.EF;

namespace IP_8IEN.DAL
{
    public class DashRepository : IDashRepository
    {
        private OurDbContext ctx;
        public bool isUoW;

        public DashRepository()
        {
            ctx = new OurDbContext();
            ctx.Database.Initialize(false);
        }

        //UoW related
        public DashRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
        }        
        public bool isUnitofWork()
        {
            return isUoW;
        }
        public void setUnitofWork(bool UoW)
        {
            isUoW = UoW;
        }

        //gebruik deze methode voor het type: 'Vergelijking','Kruising' en 'Cijfer'
        public void AddDashItem(DashItem dashItem)
        {
            ctx.DashItems.Add(dashItem);
        }
    }
}
