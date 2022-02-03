using System;
using System.Collections.Generic;
using System.Text;

namespace FShad.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustName { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public int CustomerTypeId { get; set; }
        public virtual CustomerTypes CustomerType { get; set; }
    }
}
