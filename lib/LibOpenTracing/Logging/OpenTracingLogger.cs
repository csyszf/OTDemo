using System;
using System.Collections.Generic;
using Jaeger;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace LibOpenTracing.Logging
{
    internal class OpenTracingLogger : ILogger
    {
        private const string OriginalFormatPropertyName = "{OriginalFormat}";

        private readonly ITracer _tracer;
        private readonly string _categoryName;

        public OpenTracingLogger(ITracer tracer, string categoryName)
        {
            _tracer = tracer;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // Filtering should be done via the general Logging filtering feature.

            return !_tracer.IsNoopTracer();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
            {
                // This throws an Exception e.g. in Microsoft's DebugLogger but we don't want the app to crash if the logger has an issue.
                return;
            }

            ISpan span = _tracer.ActiveSpan;

            if (span == null)
            {
                // Creating a new span for a log message seems brutal so we ignore messages if we can't attach it to an active span.
                return;
            }

            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (((Span)span).FinishTimestampUtc != null)
            {
                return;
            }

            var fields = new Dictionary<string, object>
            {
                { "component", _categoryName },
                { "level", logLevel.ToString() }
            };

            if (eventId.Id != 0)
            {
                fields["eventId"] = eventId.Id;
            }
            if (!string.IsNullOrEmpty(eventId.Name))
            {
                fields["eventName"] = eventId.Name;
            }

            string message = formatter(state, exception);
            fields[LogFields.Message] = message;

            if (exception != null)
            {
                fields[LogFields.ErrorKind] = exception.GetType().FullName;
                fields[LogFields.ErrorObject] = exception;
            }

            bool eventAdded = false;

            if (state is IEnumerable<KeyValuePair<string, object>> structure)
            {

                foreach (KeyValuePair<string, object> property in structure)
                {
                    if (string.Equals(property.Key, OriginalFormatPropertyName, StringComparison.Ordinal)
                         && property.Value is string messageTemplateString)
                    {
                        fields[LogFields.Event] = messageTemplateString;
                        eventAdded = true;
                    }
                    else
                    {
                        fields[property.Key] = property.Value;
                    }
                }
            }

            if (!eventAdded)
            {
                fields[LogFields.Event] = "log";
            }

            if (((Span)span).FinishTimestampUtc != null)
            {
                throw new InvalidOperationException("span has been finished");
            }
            // var view = new ReadOnlyDictionary<string, object>(fields);
            span.Log(fields);
        }

        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();

            public void Dispose()
            {
            }
        }
    }
}
