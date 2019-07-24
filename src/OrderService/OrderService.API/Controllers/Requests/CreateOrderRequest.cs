using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderService.API.Controllers.Requests
{
    public sealed class CreateOrderRequest
    {
        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? ZipCode { get; set; }

        [Required]
        public List<OrderItem>? OrderItems { get; set; }

        public class OrderItem
        {
            [Required]
            public int? ProductId { get; set; }

            [Required]
            public string? ProductName { get; set; }

            [Required]
            public decimal? UnitPrice { get; set; }

            [Required]
            public int? Units { get; private set; }
        }
    }
}
