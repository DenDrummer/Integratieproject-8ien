using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace IP3_8IEN.DAL.EF
{
    class OurDbConfiguration : DbConfiguration
    {
        public OurDbConfiguration()
        {
            this.SetDefaultConnectionFactory(new SqlConnectionFactory());
            this.SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
            this.SetDatabaseInitializer<OurDbContext>(new OurDbInitializer());
        }
    }
}
