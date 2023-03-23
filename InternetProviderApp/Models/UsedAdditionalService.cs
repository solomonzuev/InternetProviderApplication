using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetProviderApp.Models
{
    public class UsedAdditionalService
    {
        public Contract Contract { get; set; }
        public AdditionalService Service { get; set; }
        public bool IsServiceWork { get; set; }
        public DateTime DateOfLastPayment { get; set; }
        public string IsServiceWorkText => IsServiceWork ? "Активен" : "Не активен";
    }
}
