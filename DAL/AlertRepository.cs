using System.Collections.Generic;
using IP_8IEN.BL.Domain.Data;

namespace IP_8IEN.DAL
{
    public class AlertRepository
    {
        private List<Alert> Alerts;

        public AlertRepository()
        {
            Seed();
        }

        private void Seed()
        {
            Alerts = new List<Alert>();
            Alert a1 = new Alert(1, 1);
            Alert a2 = new Alert(2, 1);
            Alert a3 = new Alert(3, 1);
            Alert a4 = new Alert(4, 1);
            Alert a5 = new Alert(5, 1);
            Alerts.Add(a1);
            Alerts.Add(a2);
            Alerts.Add(a3);
            Alerts.Add(a4);
            Alerts.Add(a5);
        }

        public IEnumerable<Alert> ReadAlerts()
        {
            return Alerts;
        }

    }
}