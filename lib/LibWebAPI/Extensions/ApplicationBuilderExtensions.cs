using LibWebAPI.Tracing;

namespace Microsoft.AspNetCore.Builder
{
    public static class MiddlewareExtensions
    {

        public static IApplicationBuilder EnableTracing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TracingMiddleware>();
        }

    }
}
