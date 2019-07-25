using System;
using System.Linq;
using OrderService.Domain.AggregateModel.OrderAggregates;
using Xunit;

namespace OrderService.Domain.Test.Aggregates.OrderAggregate
{
    public class OrderTests
    {
        [Fact]
        public void CreateOrder()
        {
            var orderItems = Enumerable.Range(0, 19).Select(i => new OrderItem()).ToList();

            var order = new Order(Guid.NewGuid(), "TestUser", "Beijing", "100000", orderItems);
        }

        [Fact]
        public void CreateOrderShouldThrowExceptionIfHasTooMoreOrderItems()
        {
            var orderItems = Enumerable.Range(0, 21).Select(i => new OrderItem()).ToList();

            Exception exception = Assert.Throws<Exception>(() => new Order(Guid.NewGuid(), "TestUser", "Beijing", "100000", orderItems));

            Assert.Equal("An order can only have less than 20 items", exception.Message);
        }
    }
}
