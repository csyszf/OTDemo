using System;
using Microsoft.AspNetCore.Mvc;
using LibWebAPI.Tracing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcOptionsExtensions
    {
        public static IServiceCollection AddActionTracing(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.AddService<TraceActionFilter>(-200);
            });

            services.AddSingleton<TraceActionFilter>();
            return services;
        }
    }
}
