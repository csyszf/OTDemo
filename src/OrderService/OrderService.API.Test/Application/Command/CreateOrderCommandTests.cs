using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibMediator.Command;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OrderService.API.Application.Command;
using OrderService.API.Infrastructure.Services;
using OrderService.Domain.AggregatesModel.OrderAggregate;
using OrderService.Infrastructure;
using Xunit;

namespace OrderService.API.Test.Application.Command
{
    public class CreateOrderCommandTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IStockService> _stockServiceMock;

        public CreateOrderCommandTests()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<CreateOrderCommandToAggregate>()).CreateMapper();
            _stockServiceMock = new Mock<IStockService>();
        }

        [Fact]
        public async Task Handle()
        {
            // arrange
            CommandResult result = CommandResult.Ok;
            var command = new CreateOrderCommand(Guid.NewGuid(), "TestUser", "Beijing", "100001", new List<OrderItemDTO> { new OrderItemDTO(1, "IPhone", 8888, 1) });
            using (OrderDbContext context = DbHelper.GetInMemory())
            {
                CreateOrderCommand.Handler handler = GetHandler(context);

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

        [Fact]
        public async Task ShouldHoldStock()
        {
            // arrange
            CreateOrderCommand command = CreateCommand();
            _stockServiceMock
                .Setup(m => m.HoldStock(It.IsAny<Order>()))
                .Verifiable();
            using (OrderDbContext context = DbHelper.GetInMemory())
            {
                CreateOrderCommand.Handler handler = GetHandler(context);

                // act
                await handler.Handle(command);
            }

            // assert
            _stockServiceMock.Verify();
        }

        private CreateOrderCommand CreateCommand()
        {
            return new CreateOrderCommand(Guid.NewGuid(), "TestUser", "Beijing", "100001",
                new List<OrderItemDTO> { new OrderItemDTO(1, "IPhone", 8888, 1) });
        }

        private CreateOrderCommand.Handler GetHandler(OrderDbContext context) =>
            new CreateOrderCommand.Handler(NullLogger<CreateOrderCommand.Handler>.Instance,
                _mapper,
                context,
                _stockServiceMock.Object);

    }
}
