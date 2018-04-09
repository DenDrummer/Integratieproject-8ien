using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IP3_8IEN.DAL.EF;

namespace IP3_8IEN.DAL
{
    public class UnitOfWork
    {
        private OurDbContext ctx;

        internal OurDbContext Context
        {
            get
            {
                if (ctx == null) ctx = new OurDbContext(true);
                return ctx;
            }
        }

        /// <summary>
        /// Deze methode zorgt ervoor dat alle tot hier toe aangepaste domein objecten
        /// worden gepersisteert naar de databank
        /// </summary>
        public void CommitChanges()
        {
            ctx.CommitChanges();
        }
    }
}
