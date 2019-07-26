using System.Threading.Tasks;
using OrderService.Domain.AggregatesModel.OrderAggregate;

namespace OrderService.API.Infrastructure.Services
{
    public interface IStockService
    {
        Task HoldStock(Order order);
    }
}
