using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetProviderApp.Models
{
    public class Contract
    {
        public long Number { get; set; }
        public Client Client { get; set; }
        public Tariff Tariff { get; set; }
        public decimal Balance { get; set; }
        public short Discount { get; set; }
        public bool IsTariffWork { get; set; }
        public DateTime DateOfLastPayment { get; set; }

        public string IsTariffWorkText => IsTariffWork ? "Работает" : "Остановлен";
    }
}
