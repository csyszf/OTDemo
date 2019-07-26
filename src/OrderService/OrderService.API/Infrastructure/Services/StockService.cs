using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using OrderService.Domain.AggregatesModel.OrderAggregate;

namespace OrderService.API.Infrastructure.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public StockService(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task HoldStock(Order order)
        {
            HoldStockRequest req = _mapper.Map<HoldStockRequest>(order);

            var content = new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json");
            using HttpResponseMessage res = await _client.PostAsync("/api/stock/hold", content);
            res.EnsureSuccessStatusCode();
        }
    }
}
