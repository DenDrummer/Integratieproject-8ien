using System.Data.Entity;
using IP_8IEN.BL.Domain.Gebruikers;

namespace IP_8IEN.DAL.EF
{
    class OurDbInitializer : DropCreateDatabaseIfModelChanges<OurDbContext>
    {
        protected override void Seed(OurDbContext context)
        {
            base.Seed(context);
            Gebruiker g1 = new Gebruiker() {GebruikerId = 1, Email = "ivaylo.deshev@student.kdg.be"};
            Gebruiker g2 = new Gebruiker() {GebruikerId = 2, Email = "sam.laureys@student.kdg.be"};
            Gebruiker g3 = new Gebruiker() {GebruikerId = 3, Email = "jorden.laureyssens@student.kdg.be"};
            Gebruiker g4 = new Gebruiker() {GebruikerId = 4, Email = "stephane.boogaerts@student.kdg.be"};
            Gebruiker g5 = new Gebruiker() {GebruikerId = 5, Email = "thomas.dewitte@student.kdg.be"};
            Gebruiker g6 = new Gebruiker() {GebruikerId = 6, Email = "victor.declercq@student.kdg.be"};
            // Add things that will be stored in the DB


            // Save the changes in the context (all added entities) to the database
            context.SaveChanges();
        }
    }
}