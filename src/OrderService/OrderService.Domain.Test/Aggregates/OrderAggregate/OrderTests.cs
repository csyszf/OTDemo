using System;
using System.Linq;
using OrderService.Domain.AggregatesModel.OrderAggregate;
using Xunit;

namespace OrderService.Domain.Test.Aggregates.OrderAggregate
{
    public class OrderTests
    {
        [Fact]
        public void CreateOrder()
        {
            var orderItems = Enumerable.Range(0, 19).Select(i => new OrderItem(1, "IPhone", 8888, 1)).ToList();

            var order = Order.Create(Guid.NewGuid(), "TestUser", "Beijing", "100000", orderItems);
        }

        [Fact]
        public void CreateOrderShouldThrowExceptionIfHasTooMoreOrderItems()
        {
            var orderItems = Enumerable.Range(0, 21).Select(i => new OrderItem(1, "IPhone", 8888, 1)).ToList();

            Exception exception = Assert.Throws<Exception>(() => Order.Create(Guid.NewGuid(), "TestUser", "Beijing", "100000", orderItems));

            Assert.Equal("An order can only have less than 20 items", exception.Message);
        }
    }
}
