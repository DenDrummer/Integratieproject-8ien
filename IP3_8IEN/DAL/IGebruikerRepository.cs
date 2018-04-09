using System.Collections.Generic;
using IP3_8IEN.BL.Domain.Gebruikers;

namespace IP3_8IEN.DAL
{
    public interface IGebruikerRepository
    {
        //30 mrt 2018 : Stephane
        void AddingGebruiker(Gebruiker gebruiker);
        void AddingAlertInstelling(AlertInstelling alertinstelling);
        Gebruiker FindGebruiker(int userId);
        IEnumerable<Gebruiker> ReadGebruikers();

        //31 mrt 2018 : Stephane
        AlertInstelling ReadAlertInstelling(int alertInstellingId);
        void AddingAlert(Alert alert);
        void UpdateAlertInstelling(AlertInstelling alertInstelling);
        bool isUnitofWork();
        void setUnitofWork(bool UoW);

        //2 apr 2018 : Stephane
        IEnumerable<Alert> ReadAlerts();
    }
}
