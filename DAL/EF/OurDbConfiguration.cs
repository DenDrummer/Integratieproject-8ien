using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace IP_8IEN.DAL.EF
{
    class OurDbConfiguration : DbConfiguration
    {
        public OurDbConfiguration()
        {
            SetDefaultConnectionFactory(new SqlConnectionFactory());
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
            SetDatabaseInitializer(new OurDbInitializer());
        }
    }
}
