using System;
using AutoMapper;
using OrderService.Domain.AggregatesModel.OrderAggregate;

namespace OrderService.API.Infrastructure.Services
{
    public class HoldStockRequest
    {
        public Guid OrderId { get; set; }

        public class OrderStockItem
        {
            public Guid ProductId { get; set; }
            public int Units { get; set; }
        }

        public class FromOrder: Profile
        {
            public FromOrder()
            {
                CreateMap<Order, HoldStockRequest>();
                CreateMap<OrderItem, OrderStockItem>();
            }
        }
    }
}
