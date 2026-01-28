using Microsoft.EntityFrameworkCore;
using OrdersAppBackend.Models;

namespace OrdersAppBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Disable OUTPUT clause for OrderItems table due to triggers
            modelBuilder.Entity<OrderItem>()
                .ToTable(tb => tb.UseSqlOutputClause(false));
        }
    }
}
