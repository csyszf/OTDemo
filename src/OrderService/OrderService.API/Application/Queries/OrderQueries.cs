using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregatesModel.OrderAggregate;
using OrderService.Infrastructure;

namespace OrderService.API.Application.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly OrderDbContext _context;

        public OrderQueries(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> FindById(Guid id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
