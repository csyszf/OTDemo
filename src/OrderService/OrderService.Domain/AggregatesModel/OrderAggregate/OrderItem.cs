using System;

namespace OrderService.Domain.AggregatesModel.OrderAggregate
{
    public class OrderItem
    {
        public OrderItem(int productId, string productName, decimal unitPrice, int units)
        {

            if (productId == default)
                throw new Exception("An order item must have a product Id");
            ProductId = productId;

            if (productName == default)
                throw new Exception("An order item must have a product name");
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));

            if (unitPrice <= 0)
                throw new Exception("An order item must have a positive unit price");
            UnitPrice = unitPrice;

            if (units <= 0)
                throw new Exception("An order item must have a positive units");
            Units = units;
        }

        public Guid Id { get; private set; }

        public int ProductId { get; private set; }

        public string ProductName { get; private set; }

        public decimal UnitPrice { get; private set; }

        public int Units { get; private set; }
    }
}
