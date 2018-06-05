using System.Collections.Generic;
using IP3_8IEN.BL.Domain.Gebruikers;

namespace IP3_8IEN.DAL
{
    public interface IGebruikerRepository
    {
        void AddingGebruiker(Gebruiker gebruiker);
        void AddingAlertInstelling(AlertInstelling alertinstelling);
        void DeleteGebruiker(Gebruiker gebruiker);
        IEnumerable<Gebruiker> ReadGebruikers();
        
        AlertInstelling ReadAlertInstelling(int alertInstellingId);
        void AddingAlert(Alert alert);
        void UpdateAlertInstelling(AlertInstelling alertInstelling);
        bool IsUnitofWork();
        void SetUnitofWork(bool UoW);
        
        IEnumerable<Alert> ReadAlerts();
        
        IEnumerable<Gebruiker> ReadGebruikersWithAlertInstellingen();
        
        Alert ReadAlert(int alertId);
        
        IEnumerable<ValueFluctuation> ReadValueFluctuations();
        IEnumerable<PositiefNegatief> ReadPositiefNegatiefs();
        IEnumerable<HogerLager> ReadHogerLagers();
        
        void UpdateGebruiker(Gebruiker gebruiker);
        
        Gebruiker ReadGebruiker(string userId);
        IEnumerable<Gebruiker> ReadUsers();

        IEnumerable<Alert> ReadAlertsWithAlertInstellingen();
    }
}