using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetProviderApp.Models
{
    public class UsedEquipment
    {
        public Contract Contract { get; set; }
        public Equipment Equipment { get; set; }
        public DateTime StartDate { get; set; }

    }
}
