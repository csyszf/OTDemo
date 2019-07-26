using System;
using System.Threading.Tasks;
using LibOpenTracing;
using LibWebAPI.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace LibWebAPI.Tracing
{
    internal class TracingMiddleware
    {
        private readonly ITracer _tracer;
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        public TracingMiddleware(RequestDelegate next, ILogger<TracingMiddleware> logger, ITracer tracer)
        {
            _logger = logger;
            _next = next;
            _tracer = tracer;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            HttpRequest request = context.Request;

            ISpanContext extractedSpanContext = _tracer.Extract(BuiltinFormats.HttpHeaders, new RequestHeadersExtractAdapter(request.Headers));
            string operationName = "HTTP " + request.Method;

            IScope scope = _tracer.BuildSpan(operationName)
                .AsChildOf(extractedSpanContext)
                .WithTag(Tags.Component, "ICH.WebAPI")
                .WithTag(Tags.SpanKind, Tags.SpanKindServer)
                .WithTag(Tags.HttpMethod, request.Method)
                .WithTag(Tags.HttpUrl, GetDisplayUrl(request))
                .StartActive();

            context.Response.Headers["traceid"] = new StringValues(scope.Span.Context.TraceId);

            try
            {
                await _next(context);
                scope.Span.SetTag(Tags.HttpStatus, context.Response.StatusCode);
                scope.Dispose();
            }
            catch (Exception e)
            {
                scope.Span.SetException(e);
                scope.Dispose();
                await HandleException(context, e);
            }

        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            _logger.UnhandledException(ex);
            if (context.Response.HasStarted)
            {
                _logger.ResponseStartedErrorHandler();
                return Task.CompletedTask;
            }

            context.Response.StatusCode = 500;
            return context.Response.WriteAsync(string.Empty);
        }


        private static string GetDisplayUrl(HttpRequest request)
        {
            if (request.Host.HasValue)
            {
                return request.GetDisplayUrl();
            }

            return $"{request.Scheme}://{string.Empty}{request.PathBase.Value}{request.Path.Value}{request.QueryString.Value}";
        }

    }
}
