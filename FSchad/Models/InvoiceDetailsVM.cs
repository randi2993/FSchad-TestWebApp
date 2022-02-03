using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSchad.Models
{
    public class InvoiceDetailsVM
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalItbis { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public InvoiceVM Invoice { get; set; }
    }
}
