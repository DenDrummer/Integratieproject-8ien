using System.Collections.Generic;
using IP_8IEN.BL.Domain.Gebruikers;

namespace IP_8IEN.DAL
{
    public interface IGebruikerRepository
    {
        //30 mrt 2018 : Stephane
        void AddingGebruiker(Gebruiker gebruiker);
        void AddingAlertInstelling(AlertInstelling alertinstelling);
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

        //4 mei 2018 : Victor
        IEnumerable<ValueFluctuation> ReadValueFluctuations();
        IEnumerable<PositiefNegatief> ReadPositiefNegatiefs();
        IEnumerable<HogerLager> ReadHogerLagers();

        //10 mei 2018 : Stephane
        void UpdateGebruiker(Gebruiker gebruiker);

        //20 mei 2018 : Stephane
        Gebruiker ReadGebruiker(string userId);
        IEnumerable<Gebruiker> ReadUsers();
    }
}