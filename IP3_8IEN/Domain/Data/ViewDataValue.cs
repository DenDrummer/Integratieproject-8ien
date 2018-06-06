using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.BL.Domain.Data
{
    public class ViewDataValue
    {
        [Key]
        public int ViewValId;
        public string Name;
        public string StringValue;
        public int IntValue;
    }
}
