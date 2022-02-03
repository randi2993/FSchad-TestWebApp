using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSchad.Models
{
    public class CustomerVM
    {
        public int Id { get; set; }
        public string CustName { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public int CustomerTypeId { get; set; }
        public CustomerTypesVM CustomerType { get; set; }
    }
}
