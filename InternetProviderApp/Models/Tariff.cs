using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetProviderApp.Models
{
    public class Tariff
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerDay { get; set; }
        public int InternetSpeed { get; set; }
    }
}
