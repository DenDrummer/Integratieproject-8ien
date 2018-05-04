using System.Collections.Generic;
using IP_8IEN.BL.Domain.Gebruikers;

namespace IP_8IEN.DAL
{
    public interface IGebruikerRepository
    {
        //30 mrt 2018 : Stephane
        void AddingGebruiker(Gebruiker gebruiker);
        void AddingAlertInstelling(AlertInstelling alertinstelling);
        Gebruiker FindGebruiker(int userId);
        void DeleteGebruiker(Gebruiker gebruiker);
        IEnumerable<Gebruiker> ReadGebruikers();

        //31 mrt 2018 : Stephane
        AlertInstelling ReadAlertInstelling(int alertInstellingId);
        void AddingAlert(Alert alert);
        void UpdateAlertInstelling(AlertInstelling alertInstelling);
        bool isUnitofWork();
        void setUnitofWork(bool UoW);

        //2 apr 2018 : Stephane
        IEnumerable<Alert> ReadAlerts();

        //4 mei 2018 : Victor
        IEnumerable<Gebruiker> ReadGebruikersWithAlertInstellingen();

        //4 mei 2018 : Stephane
        Alert ReadAlert(int alertId);
    }
}
