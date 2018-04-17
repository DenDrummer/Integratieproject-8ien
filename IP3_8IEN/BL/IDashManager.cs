using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.BL
{
    public interface IDashManager
    {
        //4 apr 2018 : Stephane
        void initNonExistingRepo(bool withUnitOfWork);
    }
}
