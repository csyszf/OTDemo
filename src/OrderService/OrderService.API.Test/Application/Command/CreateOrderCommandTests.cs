using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibMediator.Command;
using Microsoft.Extensions.Logging.Abstractions;
using OrderService.API.Application.Command;
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
            var command = new CreateOrderCommand(Guid.NewGuid(), "TestUser", "Beijing", "100001", new List<OrderItemDTO> { new OrderItemDTO(1, "IPhone", 8888, 1) });
            var handler = new CreateOrderCommand.Handler(NullLogger<CreateOrderCommand.Handler>.Instance, _mapper);

            CommandResult result = await handler.Handle(command);

            Assert.True(result.Succeed);
        }
    }
}
