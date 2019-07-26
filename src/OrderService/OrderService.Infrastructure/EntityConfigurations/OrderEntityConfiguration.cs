using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregatesModel.OrderAggregate;

namespace OrderService.Infrastructure.EntityConfigurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id);
            builder.Property(e => e.Status);
            builder.Property(e => e.UserId);
            builder.Property(e => e.UserName);
            builder.Property(e => e.City);
            builder.Property(e => e.ZipCode);
            //builder.OwnsMany(e => e.OrderItems, c=>
            //{
            //    builder.ToTable("OrderItems");

            //    builder.HasKey(e => e.Id);
            //    builder.Property<Guid>("OrderId");
            //});
            builder.HasMany(e => e.OrderItems).WithOne().HasForeignKey("OrderId");
            //builder.Property(e => e.OrderItems);
            //builder.HasMany(e => e.OrderItems).WithOne().HasForeignKey("OrderId");
        }
    }
}
