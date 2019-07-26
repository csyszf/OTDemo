using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregatesModel.OrderAggregate;

namespace OrderService.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; private set; } = default!;
        public DbSet<OrderItem> OrderItems { get; private set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
