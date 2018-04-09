using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IP3_8IEN.BL.Domain.Dashboard;

namespace IP3_8IEN.DAL
{
    public interface IDashRepository
    {
        //5 apr 2018
        bool isUnitofWork();
        void setUnitofWork(bool UoW);
        void AddDashItem(DashItem dashItem);

    }
}
