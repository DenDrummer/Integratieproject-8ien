using System.Data.Entity;

namespace IP3_8IEN.DAL.EF
{
    class OurDbInitializer : CreateDatabaseIfNotExists<OurDbContext>
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