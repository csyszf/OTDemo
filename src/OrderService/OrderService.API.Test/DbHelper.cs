using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure;

namespace OrderService.API.Test
{
    public class DbHelper
    {
        private const string _connectionString = "DataSource=:memory:";

        public static OrderDbContext GetInMemory([CallerFilePath] string callerName = "Unknown")
        {
            DbContextOptions<OrderDbContext> options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(callerName)
                .Options;

            return new OrderDbContext(options);
        }

        public static OrderDbContext Sqlite
        {
            get
            {
                DbContextOptions<OrderDbContext> options = new DbContextOptionsBuilder<OrderDbContext>()
                    .UseSqlite(_connectionString)
                    .Options;

                var context = new OrderDbContext(options);
                context.Database.EnsureCreated();
                return context;
            }
        }
    }
}
