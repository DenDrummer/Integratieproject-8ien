using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IP_8IEN.BL.Domain.Data;

namespace IP_8IEN.DAL
{
    public class DataRepository
    {
        private readonly EF.OurDbContext ctx;

        public DataRepository()
        {
            ctx = new EF.OurDbContext();
        }

        public Onderwerp ReadOnderwerp(int onderwerpId)
        {
            return ctx.Onderwerpen.Find(onderwerpId);

        }

        public IEnumerable<Onderwerp> ReadAllOnderwerpen()
        {
            return ctx.Onderwerpen.ToList();
        }
    }
}
