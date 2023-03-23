using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetProviderApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string HomeAddress { get; set; }
        public string PassportNumber { get; set; }
        public string CodPorazdeleniya { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
