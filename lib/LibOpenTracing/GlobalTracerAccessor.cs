using OpenTracing;
using OpenTracing.Util;

namespace LibOpenTracing
{
    public class GlobalTracerAccessor : IGlobalTracerAccessor
    {
        public ITracer GetGlobalTracer()
        {
            return GlobalTracer.Instance;
        }
    }
}
