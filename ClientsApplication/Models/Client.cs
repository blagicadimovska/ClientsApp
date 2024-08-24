using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsApplication.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClientID { get; set; }
        public string Name { get; set; }
        public List<Address> Addresses { get; set; }
        public DateTime BirthDate { get; set; }

        public Client()
        {
           Addresses = new List<Address>();
        }

    }
}
