using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetProviderApp.Models
{
    public class AdditionalService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerDay { get; set; }
        public string Description { get; set; }
    }
}
