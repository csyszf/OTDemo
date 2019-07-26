using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderService.Infrastructure;

namespace OrderService.IntegrationTest
{
    public class WebApplicationFactoryWithInMemorySqlite<TStartup> : WebApplicationFactory<TStartup>
            where TStartup : class
    {
        private readonly string _connectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        public WebApplicationFactoryWithInMemorySqlite()
        {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSolutionRelativeContentRoot(Path.Combine("src", "OrderService", "OrderService.API"));

            builder.ConfigureServices(services =>
            {

                // EFCore add DbContextOptions by `TryAdd`
                // and Startup's ConfigureServices is called before here
                // so we need to clean up previous options
                services.RemoveAll<DbContextOptions>();
                services.RemoveAll<DbContextOptions<OrderDbContext>>();

                ServiceProvider serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkSqlite()
                    .BuildServiceProvider();
                services.AddDbContext<OrderDbContext>(options =>
                {
                    options.UseSqlite(_connection);
                    options.UseInternalServiceProvider(serviceProvider);
                });

                ServiceProvider sp = services.BuildServiceProvider();

                using (IServiceScope scope = sp.CreateScope())
                {
                    System.IServiceProvider scopedServices = scope.ServiceProvider;
                    OrderDbContext db = scopedServices.GetRequiredService<OrderDbContext>();
                    db.Database.EnsureCreated();
                }
            });

        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _connection.Close();
        }
    }
}
