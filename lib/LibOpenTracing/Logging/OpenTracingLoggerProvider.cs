using System;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace LibOpenTracing.Logging
{
    [ProviderAlias("OpenTracing")]
    public class OpenTracingLoggerProvider : ILoggerProvider
    {
        private readonly ITracer _tracer;

        public OpenTracingLoggerProvider(IGlobalTracerAccessor globalTracerAccessor)
        {
            // HACK: We can't use ITracer directly here because this would lead to a StackOverflowException
            // (due to a circular dependency) if the ITracer needs a ILoggerFactory.
            // https://github.com/opentracing-contrib/csharp-netcore/issues/14

            if (globalTracerAccessor == null)
            {
                throw new ArgumentNullException(nameof(globalTracerAccessor));
            }

            _tracer = globalTracerAccessor.GetGlobalTracer();
        }

        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
        {
            return new OpenTracingLogger(_tracer, categoryName);
        }

        public void Dispose()
        {
        }
    }
}
