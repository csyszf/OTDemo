using LibOpenTracing;
using LibOpenTracing.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Microsoft.Extensions.Logging
{
    public static class OpenTracingLoggerExtensions
    {
        public static IServiceCollection AddOpenTracingLogger(this IServiceCollection services)
        {
            services.TryAddSingleton<IGlobalTracerAccessor, GlobalTracerAccessor>();
            services.AddLogging(logging =>
            {
                logging.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, OpenTracingLoggerProvider>());
                logging.AddFilter<OpenTracingLoggerProvider>("Default", LogLevel.Information);
                logging.AddFilter<OpenTracingLoggerProvider>("Microsoft.AspNetCore.Mvc", LogLevel.Warning);
            });
            return services;
        }
    }
}
