using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.API.Application.Queries;
using OrderService.API.Infrastructure.Services;
using OrderService.Infrastructure;

namespace OrderService.API
{
    public class Startup
    {

        private readonly SqliteConnection _connection = new SqliteConnection("DataSource=:memory:");

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visits https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .TraceActions();

            services.AddHealthChecks();

            ConfigureTracing(services);
            ConfigureDatabase(services);

            services.AddMediator(typeof(Startup));
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IOrderQueries, OrderQueries>();

            ConfigureServiceClients(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _connection.Open();
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            scope.ServiceProvider.GetRequiredService<OrderDbContext>().Database.EnsureCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/hc");

            app.EnableTracing();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlite(_connection));
        }

        private void ConfigureServiceClients(IServiceCollection services)
        {
            services.AddHttpClient("stock", client =>
                {
                    client.BaseAddress = new Uri(Configuration["Endpoints:StockService"]);
                })
                .WithTracing()
                .AddTypedClient<IStockService, StockService>();
        }

        private void ConfigureTracing(IServiceCollection services)
        {
            services.AddOpenTracingLogger();
            services.AddTracing(Env.ApplicationName, Configuration["Tracing:Endpoint"]);
        }
    }
}
