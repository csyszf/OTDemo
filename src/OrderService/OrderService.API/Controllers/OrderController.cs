using System.Threading.Tasks;
using AutoMapper;
using LibMediator;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.Application.Command;
using OrderService.API.Controllers.Requests;

namespace OrderService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            CreateOrderCommand command = _mapper.Map<CreateOrderCommand>(request);
            LibMediator.Command.CommandResult result = await _mediator.Send(command);
            if (result.Succeed)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
