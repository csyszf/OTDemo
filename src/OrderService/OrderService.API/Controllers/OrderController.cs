using System.Threading.Tasks;
using AutoMapper;
using LibMediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderService.API.Application.Command;
using OrderService.API.Controllers.Requests;

namespace OrderService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderController(ILogger<OrderController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
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
