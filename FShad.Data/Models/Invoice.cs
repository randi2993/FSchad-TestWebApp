using System;
using System.Collections.Generic;
using System.Text;

namespace FShad.Data.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public Decimal TotalItbis { get; set; }
        public Decimal SubTotal { get; set; }
        public Decimal Total { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
