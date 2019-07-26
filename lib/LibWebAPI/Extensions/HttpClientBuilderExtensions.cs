using LibWebAPI.Tracing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTracing;
using OpenTracing.Noop;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder WithTracing(this IHttpClientBuilder clientBuilder)
        {
            clientBuilder.Services.TryAddSingleton<ITracer>(NoopTracerFactory.Create());
            clientBuilder.Services.AddTransient<TracingHttpMessageHandler>();
            clientBuilder.AddHttpMessageHandler<TracingHttpMessageHandler>();
            return clientBuilder;
        }
    }
}
