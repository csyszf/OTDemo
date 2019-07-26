using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace StockService.API.Controllers
{
    [Route("api/stock/hold")]
    [ApiController]
    public class OrderStockController: ControllerBase
    {
        private readonly ILogger<OrderStockController> _logger;

        public OrderStockController(ILogger<OrderStockController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> HoldStockForOrder()
        {
            _logger.LogInformation("Hold stock for order started");

            await Task.Delay(10);

            _logger.LogInformation("Hold stock for order succeed");
            return Ok();
        }
    }
}
