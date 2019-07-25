using System;
using System.Collections.Generic;

namespace OrderService.Domain.AggregateModel.OrderAggregates
{
    public class Order
    {
        public Order(Guid userId, string userName, string city, string zipCode, ICollection<OrderItem> orderItems)
        {
            if (Guid.Empty == userId)
            {
                throw new Exception("UserId can not be empty");
            }

            UserId = userId;

            if (string.Empty == userName)
            {
                throw new Exception("User name can not be empty");
            }

            UserName = userName;

            if (string.Empty == city)
            {
                throw new Exception("City can not be empty");
            }

            City = city;

            if (string.Empty == zipCode)
            {
                throw new Exception("Zip code can not be empty");
            }

            ZipCode = zipCode;


            if (orderItems.Count == 0)
            {
                throw new Exception("Order must has items");
            }

            if (orderItems.Count > 20)
            {
                throw new Exception("An order can only have less than 20 items");
            }

            OrderItems = orderItems;
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public string UserName { get; private set; }

        public string City { get; private set; }

        public string ZipCode { get; private set; }

        public ICollection<OrderItem> OrderItems { get; private set; }
    }
}
