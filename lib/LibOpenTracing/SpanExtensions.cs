using System;
using System.Collections.Generic;
using OpenTracing;
using OpenTracing.Tag;

namespace LibOpenTracing
{
    public static class SpanExtensions
    {
        /// <summary>
        /// Sets the <see cref="Tags.Error"/> tag and adds information about the <paramref name="exception"/>
        /// to the given <paramref name="span"/>.
        /// </summary>
        public static void SetException(this ISpan span, Exception exception)
        {
            if (span == null || exception == null)
            {
                return;
            }

            span.SetTag(Tags.Error, true);

            span.Log(new Dictionary<string, object>(3)
            {
                { LogFields.Event, Tags.Error.Key },
                { LogFields.ErrorKind, exception.GetType().Name },
                { LogFields.ErrorObject, exception }
            });
        }

        public static ISpanBuilder WithBusinessId(this ISpanBuilder builder, string aid)
        {
            if (string.IsNullOrEmpty(aid))
            {
                throw new ArgumentException(nameof(aid));
            }

            return builder.WithTag(OpenTracingConstants.BUSINESS_ID_TAG, aid);
        }

        public static ISpan SetBusinessId(this ISpan span, string aid)
        {
            if (string.IsNullOrEmpty(aid))
            {
                throw new ArgumentException(nameof(aid));
            }

            return span.SetTag(OpenTracingConstants.BUSINESS_ID_TAG, aid);
        }
    }
}
