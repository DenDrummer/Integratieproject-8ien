using IP_8IEN.BL.Domain.Dashboard;
using IP_8IEN.BL.Domain.Data;
using IP_8IEN.BL.Domain.Gebruikers;
using IP3_8IEN.BL.Domain.Dashboard;
using System.Collections.Generic;

namespace IP_8IEN.BL
{
    public interface IDashManager
    {
        //4 apr 2018 : Stephane
        void initNonExistingRepo(bool withUnitOfWork);

        //10 mei 2018 : Stephane
        Dashbord GetDashboard(Gebruiker user);
        DashItem AddDashItem(Gebruiker user, Onderwerp onderwerp);
        void AddGraph(GraphData graph);
        Dashbord AddDashBord(Gebruiker gebruiker);
    }
}
