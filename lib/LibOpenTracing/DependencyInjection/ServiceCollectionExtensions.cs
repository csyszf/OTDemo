using Jaeger.Samplers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Noop;
using OpenTracing.Util;
using JaegerConfiguration = Jaeger.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTracing(this IServiceCollection services,
            string serviceName,
            string agentHost)
        {
            return services.AddTracing(serviceName, agentHost, null, false, 1024);
        }

        public static IServiceCollection AddTracing(this IServiceCollection services,
            string serviceName,
            string agentHost,
            int maxQueueSize)
        {
            return services.AddTracing(serviceName, agentHost, null, false, maxQueueSize);
        }

        public static IServiceCollection AddTracing(this IServiceCollection services,
            string serviceName,
            string agentHost,
            int? agentPort,
            bool logSpans,
            int? maxQueueSize)
        {
            services.RemoveAll<ITracer>();
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                JaegerConfiguration.SenderConfiguration senderConfiguration = new JaegerConfiguration.SenderConfiguration(loggerFactory)
                    .WithAgentHost(agentHost)
                    .WithAgentPort(agentPort);
                JaegerConfiguration.ReporterConfiguration reporterConfiguration = new JaegerConfiguration.ReporterConfiguration(loggerFactory)
                    .WithLogSpans(logSpans)
                    .WithMaxQueueSize(maxQueueSize)
                    .WithSender(senderConfiguration);
                JaegerConfiguration.SamplerConfiguration samplerConfiguration = new JaegerConfiguration.SamplerConfiguration(loggerFactory)
                    .WithType(ConstSampler.Type)
                    .WithParam(1);
                var tracer = (Jaeger.Tracer)new JaegerConfiguration(serviceName, loggerFactory)
                    .WithSampler(samplerConfiguration)
                    .WithReporter(reporterConfiguration)
                    .GetTracer();

                GlobalTracer.Register(tracer);

                return tracer;
            });

            return services;
        }

        public static IServiceCollection AddNoopTracing(this IServiceCollection services)
        {
            services.AddSingleton<ITracer>(NoopTracerFactory.Create());
            return services;
        }
    }
}
