using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsApplication.Models
{
    public class Address
    {
        public int AddressID { get; set; }
        public int ClientID { get; set; } // Foreign key to Client
        public int Type { get; set; }
        public string ClientAddress { get; set; }
    }
}
