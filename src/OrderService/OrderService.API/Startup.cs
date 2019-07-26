using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.API.Infrastructure.Services;
using OrderService.Infrastructure;

namespace OrderService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks();

            ConfigureDatabase(services);

            services.AddMediator(typeof(Startup));
            services.AddAutoMapper(typeof(Startup));

            ConfigureServiceClients(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/hc");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<OrderDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection"))
                    );
        }

        private void ConfigureServiceClients(IServiceCollection services)
        {
            services.AddHttpClient("stock", client =>
                {
                    client.BaseAddress = new Uri(Configuration["Endpoints:StockService"]);
                })
                .AddTypedClient<IStockService, StockService>();
        }
    }
}
