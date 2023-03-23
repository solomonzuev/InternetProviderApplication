using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InternetProviderApp.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public short WarrantyInMonths { get; set; }
        public byte[] ImagePreview { get; set; }
    }
}
