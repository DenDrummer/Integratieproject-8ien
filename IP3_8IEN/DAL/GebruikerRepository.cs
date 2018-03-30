using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IP3_8IEN.BL.Domain.Gebruikers;

namespace IP3_8IEN.DAL
{
    public class GebruikerRepository
    {
        private List<Gebruiker> Gebruikers;

        public GebruikerRepository()
        {

            Seed();
        }

        private void Seed()
        {
            Gebruikers = new List<Gebruiker>();
            Gebruiker g1 = new Gebruiker() { GebruikerId = 1, Email = "ivaylo.deshev@student.kdg.be" };
            Gebruiker g2 = new Gebruiker() { GebruikerId = 2, Email = "sam.laureys@student.kdg.be" };
            Gebruiker g3 = new Gebruiker() { GebruikerId = 3, Email = "jorden.laureyssens@student.kdg.be" };
            Gebruiker g4 = new Gebruiker() { GebruikerId = 4, Email = "stephane.boogaerts@student.kdg.be" };
            Gebruiker g5 = new Gebruiker() { GebruikerId = 5, Email = "thomas.dewitte@student.kdg.be" };
            Gebruiker g6 = new Gebruiker() { GebruikerId = 6, Email = "victor.declercq@student.kdg.be" };
            // Add things that will be stored in the DB
            Gebruikers.Add(g1);
            Gebruikers.Add(g2);
            Gebruikers.Add(g3);
            Gebruikers.Add(g4);
            Gebruikers.Add(g5);
            Gebruikers.Add(g6);
        }

        public IEnumerable<Gebruiker> ReadGebruikers()
        {
            return Gebruikers.ToList();
        }

        //        private readonly OurDbContext ctx;
        //
        //        public GebruikerRepository()
        //        {
        //            ctx = new OurDbContext();
        //        }
        //
        //        public GebruikerRepository(UnitOfWork uow)
        //        {
        //            ctx = uow.context;
        //        }
        //
        //        public IEnumerable<Gebruiker> ReadGebruikers()
        //        {
        //            return ctx.Gebruikers.ToList();
        //        }
    }
}
