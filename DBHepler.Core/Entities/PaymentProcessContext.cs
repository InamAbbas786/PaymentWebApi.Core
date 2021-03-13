using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBHepler.Core.Entities
{
    public class PaymentProcessContext : DbContext
    {
        public PaymentProcessContext(DbContextOptions<PaymentProcessContext> options):base (options)
        {

        }

        public DbSet<ProcessPayment> ProcessPayments { get; set; }
        public DbSet<PaymentState> PaymentStates { get; set; }
        public DbSet<PaymentLogs> PaymentLogs { get; set; }
    }
}
