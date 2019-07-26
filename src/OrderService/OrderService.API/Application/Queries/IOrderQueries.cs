using System;
using System.Threading.Tasks;
using OrderService.Domain.AggregatesModel.OrderAggregate;

namespace OrderService.API.Application.Queries
{
    public interface IOrderQueries
    {
        Task<Order?> FindById(Guid id);
    }
}