using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IP_8IEN.UI_MVC.Models
{
    public class IP_8IENUI_MVCContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public IP_8IENUI_MVCContext() : base("name=IP_8IENUI_MVCContext")
        {
        }

        public System.Data.Entity.DbSet<IP_8IEN.BL.Domain.Data.Alert> Alerts { get; set; }
    }
}
