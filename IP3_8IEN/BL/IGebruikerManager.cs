using IP3_8IEN.BL.Domain.Gebruikers;
using System.Collections.Generic;

namespace IP3_8IEN.BL
{
    public interface IGebruikerManager
    {
        //void AddGebruikers(string filePath);
        void AddAlertInstelling(string filePath);
        Gebruiker FindUser(string username);
        
        void AddAlerts(string filePath);
        void AddAlert(string alertContent, int alertInstelling);
        void InitNonExistingRepo(bool withUnitOfWork);
        
        IEnumerable<Alert> GetAlerts();
        
        void WeeklyReview();
        
        Alert GetAlert(int alertId);
        
        void GetAlertHogerLagers();
        
        void GetAlertValueFluctuations();
        void GetAlertPositiefNegatiefs();
        
        List<HogerLager> GetHogerLagersByUser();
        List<PositiefNegatief> GetPositiefNegatiefsByUser();
        List<ValueFluctuation> GetValueFluctuationsByUser();
        List<Alert> GetAlertsByUser(Gebruiker gebruiker);
        //void AddApplicationGebruikers(string filePath); <-- verhuist naar ApplicationUserManager()

        //10 mei 2018 : Stephane
        void AddGebruiker(string userName, string id, string naam, string voornaam, string email, string role);
        void UpdateGebruiker(Gebruiker gebruiker);
        
        void DeleteUser(string userId);
        IEnumerable<Gebruiker> GetUsers();
        IEnumerable<ApplicationUser> GetUsersInRoles(IEnumerable<ApplicationUser> appUsers, string role);
        string ExportToCSV(IEnumerable<Gebruiker> gebruikers);

    }
}