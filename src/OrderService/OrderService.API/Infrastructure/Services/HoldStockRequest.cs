using System;

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
    }
}
