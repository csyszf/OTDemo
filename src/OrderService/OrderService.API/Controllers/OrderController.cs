using System;
using System.Threading.Tasks;
using AutoMapper;
using LibMediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderService.API.Application.Command;
using OrderService.API.Application.Queries;
using OrderService.API.Controllers.Requests;
using OrderService.Domain.AggregatesModel.OrderAggregate;

namespace OrderService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrderQueries _queries;

        public OrderController(ILogger<OrderController> logger, IMediator mediator, IMapper mapper, IOrderQueries queries)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _queries = queries;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Order>> Get(Guid id)
        {
            Order? order = await _queries.FindById(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            CreateOrderCommand command = _mapper.Map<CreateOrderCommand>(request);

            _logger.LogInformation("Begin to Create Order");
            LibMediator.Command.CommandResult result = await _mediator.Send(command);
            if (result.Succeed)
            {
                _logger.LogInformation("Order Created");
                return NoContent();
            }

            _logger.LogWarning("Create Order Failed");
            return BadRequest();
        }
    }
}
