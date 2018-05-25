using IP_8IEN.BL.Domain.Gebruikers;
using System.Collections.Generic;

namespace IP_8IEN.BL
{
    public interface IGebruikerManager
    {
        //30 mrt 2018 : stephane
        //void AddGebruikers(string filePath);
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

        //8 mei 2018 : Victor
        void GetAlertHogerLagers();

        //15 mei 2018 : Victor
        void GetAlertValueFluctuations();
        void GetAlertPositiefNegatiefs();

        //18 mei 2018 : Victor
        List<HogerLager> GetHogerLagersByUser();
        List<PositiefNegatief> GetPositiefNegatiefsByUser();
        List<ValueFluctuation> GetValueFluctuationsByUser();
        List<Alert> GetAlertsByUser(Gebruiker gebruiker);
        //6 mei 2018 : Stephane
        //void AddApplicationGebruikers(string filePath); <-- verhuist naar ApplicationUserManager()

        //10 mei 2018 : Stephane
        void AddGebruiker(string userName, string id, string naam, string voornaam, string role);
        void UpdateGebruiker(Gebruiker gebruiker);

        //21 mei 2018 : Stephane
        void DeleteUser(string userId);
        IEnumerable<Gebruiker> GetUsers();
        IEnumerable<ApplicationUser> GetUsersInRoles(IEnumerable<ApplicationUser> appUsers, string role);
    }
}