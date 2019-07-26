using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibMediator.Command;
using Microsoft.Extensions.Logging;
using OrderService.Domain.AggregatesModel.OrderAggregate;
using OrderService.Infrastructure;

namespace OrderService.API.Application.Command
{
    public class CreateOrderCommand : ICommand
    {
        public CreateOrderCommand(Guid? userId, string? userName, string? city, string? zipCode, List<OrderItemDTO>? orderItems)
        {
            OrderItems = orderItems ?? throw new ArgumentNullException(nameof(orderItems));
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            City = city ?? throw new ArgumentNullException(nameof(city));
            ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        }

        public Guid UserId { get; private set; }

        public string UserName { get; private set; }

        public string City { get; private set; }

        public string ZipCode { get; private set; }

        public List<OrderItemDTO> OrderItems { get; private set; }

        public class Handler : ICommandHandler<CreateOrderCommand>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IMapper _mapper;
            private readonly OrderDbContext _context;

            public Handler(
                ILogger<Handler> logger, 
                IMapper mapper,
                OrderDbContext context)
            {
                _logger = logger;
                _mapper = mapper;
                _context = context;
            }

            public async Task<CommandResult> Handle(CreateOrderCommand command)
            {
                Order order = _mapper.Map<Order>(command);

                await _context.AddAsync(order);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Order record created with Id {0}", order.Id);
                return CommandResult.Ok;
            }
        }
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

    public class CreateOrderCommandToAggregate : Profile
    {
        public CreateOrderCommandToAggregate()
        {
            CreateMap<CreateOrderCommand, Order>();
            CreateMap<OrderItemDTO, OrderItem>();
        }
    }

}
