using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LibOpenTracing;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace LibWebAPI.Tracing
{
    public class TracingHttpMessageHandler : DelegatingHandler
    {
        private readonly ITracer _tracer;
        public TracingHttpMessageHandler(ITracer tracer)
        {
            _tracer = tracer;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ISpan span = _tracer.BuildSpan("HttpRequest")
                .WithTag(Tags.SpanKind, Tags.SpanKindClient)
                .WithTag(Tags.Component, "LibWebAPI.Http")
                .WithTag(Tags.HttpMethod, request.Method.ToString())
                .WithTag(Tags.HttpUrl, request.RequestUri.ToString())
                .WithTag(Tags.PeerHostname, request.RequestUri.Host)
                .WithTag(Tags.PeerPort, request.RequestUri.Port)
                .Start();

            _tracer.Inject(span.Context, BuiltinFormats.HttpHeaders, new HttpHeadersInjectAdapter(request.Headers));

            HttpResponseMessage? response = null;
            Task<HttpResponseMessage>? requestTask = null;
            try
            {
                requestTask = base.SendAsync(request, cancellationToken);
                response = await requestTask;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                span.SetException(e);
                throw;
            }
            finally
            {

                if (response != null)
                {
                    span.SetTag(Tags.HttpStatus, (int)response.StatusCode);
                }

                if (requestTask != null)
                {
                    TaskStatus? requestTaskStatus = requestTask?.Status;
                    if (requestTaskStatus == TaskStatus.Canceled || requestTaskStatus == TaskStatus.Faulted)
                    {
                        span.SetTag(Tags.Error, true);
                    }
                }
                span.Finish();
            }

            return response;
        }
    }
}
