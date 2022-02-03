using System;
using System.Collections.Generic;
using System.Text;

namespace FShad.Data.Models
{
    public class InvoiceDetails
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public Decimal Price { get; set; }
        public Decimal TotalItbis { get; set; }
        public Decimal SubTotal { get; set; }
        public Decimal Total { get; set; }
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
