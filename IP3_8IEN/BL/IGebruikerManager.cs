using IP_8IEN.BL.Domain.Gebruikers;
using System.Collections.Generic;

namespace IP_8IEN.BL
{
    public interface IGebruikerManager
    {
        //30 mrt 2018 : stephane
        void AddGebruikers(string filePath);
        void AddAlertInstelling(string filePath);
        Gebruiker FindUser(string username);

        //31 mrt 2018 : Stephane
        void AddAlerts(string filePath);
        void AddAlert(string alertContent, int alertInstelling);
        void initNonExistingRepo(bool withUnitOfWork);

        //2 apr 2018 : Stephane
        IEnumerable<Alert> GetAlerts();

        //4 apr 2018 : Victor
        void WeeklyReview();

        //4 mei 2018 : Stephane
        Alert GetAlert(int alertId);

        //6 mei 2018 : Stephane
        //void AddApplicationGebruikers(string filePath);
    }
}
