using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregatesModel.OrderAggregate;

namespace OrderService.Infrastructure.EntityConfigurations
{
    public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(e => e.Id);
            builder.Property<Guid>("OrderId");

            builder.Property(e => e.Id);
            builder.Property(e => e.ProductId);
            builder.Property(e => e.ProductName);
            builder.Property(e => e.UnitPrice);
            builder.Property(e => e.Units);
        }
    }
}
