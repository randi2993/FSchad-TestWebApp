using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSchad.Models
{
    public class InvoiceVM
    {
        public int Id { get; set; }
        public double TotalItbis { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }
        public int CustomerId { get; set; }
        public CustomerVM Customer { get; set; }
    }
}
