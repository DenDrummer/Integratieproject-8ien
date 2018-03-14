using System.Data.Entity;

namespace IP_8IEN.DAL.EF
{
    class OurDbInitializer : DropCreateDatabaseIfModelChanges<OurDbContext>
    {
        protected override void Seed(OurDbContext context)
        {
            //base.Seed(context);

            // Add things that will be stored in the DB

            // Save the changes in the context (all added entities) to the database
            context.SaveChanges();
        }
    }
}
