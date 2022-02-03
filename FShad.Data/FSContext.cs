using FShad.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FShad.Data
{
    public class FSContext : DbContext
    {
        public FSContext(DbContextOptions<FSContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerTypes> CustomerTypes { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceDetails> InvoiceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<CustomerTypes>().ToTable("CustomerTypes");
            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<InvoiceDetails>().ToTable("InvoiceDetail");
        }
    }
}
