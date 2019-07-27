using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;

namespace LibWebAPI.Tracing
{
    public class TraceActionFilter : IActionFilter
    {
        private const string ActionComponent = "AspNetCore.MvcAction";
        private const string ActionTagActionName = "action";
        private const string ActionTagControllerName = "controller";

        private const string ErrorCodeTag = "error_code";

        private readonly ITracer _tracer;
        private readonly ILogger _logger;

        public TraceActionFilter(ITracer tracer, ILogger<TraceActionFilter> logger)
        {
            _tracer = tracer ?? throw new ArgumentNullException(nameof(tracer));
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = context.ActionDescriptor;

            string operationName = actionDescriptor is ControllerActionDescriptor controllerActionDescriptor
                ? $"Action {controllerActionDescriptor.ControllerTypeInfo.Name}/{controllerActionDescriptor.ActionName}"
                : $"Action {actionDescriptor.DisplayName}";

            _tracer.BuildSpan(operationName)
                .WithTag(Tags.Component, ActionComponent)
                .StartActive();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var scope = _tracer.ScopeManager.Active;
            if (scope == null) return;

            var span = scope.Span;
            scope.Dispose();
        }
    }
}
