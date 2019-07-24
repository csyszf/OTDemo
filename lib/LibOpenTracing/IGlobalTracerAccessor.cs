using OpenTracing;

namespace LibOpenTracing
{
    public interface IGlobalTracerAccessor
    {
        ITracer GetGlobalTracer();
    }
}
