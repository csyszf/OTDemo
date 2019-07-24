using System;
using System.Collections.Generic;
using LibMediator.Command;

namespace OrderService.API.Application.Command
{
    public class CreateOrderCommand : ICommand
    {
        private readonly List<OrderItemDTO> _orderItems;

        public CreateOrderCommand(List<OrderItemDTO>? orderItems, string? userId, string? userName, string? city, string? zipCode)
        {
            _orderItems = orderItems ?? throw new ArgumentNullException(nameof(orderItems));
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            City = city ?? throw new ArgumentNullException(nameof(city));
            ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        }

        public string UserId { get; private set; }

        public string UserName { get; private set; }

        public string City { get; private set; }

        public string ZipCode { get; private set; }

        public IEnumerable<OrderItemDTO> OrderItems => _orderItems;

    }

    public class OrderItemDTO
    {
        public OrderItemDTO(int? productId, string? productName, decimal? unitPrice, int? units)
        {
            ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
            Units = units ?? throw new ArgumentNullException(nameof(units));
        }

        public int ProductId { get; private set; }

        public string ProductName { get; private set; }

        public decimal UnitPrice { get; private set; }

        public int Units { get; private set; }
    }
}
