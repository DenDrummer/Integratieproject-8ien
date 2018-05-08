using System.Data.Entity;

namespace IP_8IEN.DAL.EF
{
    class OurDbInitializer : DropCreateDatabaseIfModelChanges<OurDbContext>
    //DropCreateDatabaseIfModelChanges
    //DropCreateDatabaseAlways
    //CreateDatabaseIfNotExists
    {

        protected override void Seed(OurDbContext context)
        {
            base.Seed(context);
            

        }
    }
}
