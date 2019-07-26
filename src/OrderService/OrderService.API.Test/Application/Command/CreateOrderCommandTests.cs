using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibMediator.Command;
using Microsoft.Extensions.Logging.Abstractions;
using OrderService.API.Application.Command;
using OrderService.Domain.AggregatesModel.OrderAggregate;
using OrderService.Infrastructure;
using Xunit;

namespace OrderService.API.Test.Application.Command
{
    public class CreateOrderCommandTests
    {
        private readonly IMapper _mapper;

        public CreateOrderCommandTests()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<CreateOrderCommandToAggregate>()).CreateMapper();
        }

        [Fact]
        public async Task Handle()
        {
            // arrange
            CommandResult result = CommandResult.Ok;
            var command = new CreateOrderCommand(Guid.NewGuid(), "TestUser", "Beijing", "100001", new List<OrderItemDTO> { new OrderItemDTO(1, "IPhone", 8888, 1) });
            using (OrderDbContext context = DbHelper.GetInMemory())
            {
                var handler = new CreateOrderCommand.Handler(NullLogger<CreateOrderCommand.Handler>.Instance, _mapper, context);

                // act
                result = await handler.Handle(command);
            }

            // assert
            Assert.True(result.Succeed);
            using (OrderDbContext context = DbHelper.GetInMemory())
            {
                Assert.Equal(1, context.Orders.Count());
                Order createdOrder = context.Orders.Single();
                Assert.Equal(command.UserId, createdOrder.UserId);
            }
        }
    }
}
