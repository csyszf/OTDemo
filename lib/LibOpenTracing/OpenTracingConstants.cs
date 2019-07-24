namespace LibOpenTracing
{
    public static class OpenTracingConstants
    {
        public const string X_REQUEST_ID = "x-request-id";
        public const string X_B3_TRACEID = "x-b3-traceid";
        public const string X_B3_SPANID = "x-b3-spanid";
        public const string X_B3_PARENTSPANID = "x-b3-parentspanid";
        public const string X_B3_SAMPLED = "x-b3-sampled";
        public const string X_B3_FLAGS = "x-b3-flags";
        public const string X_OT_SPAN_CONTEXT = "x-ot-span-context";

        public const string BUSINESS_ID_TAG = "aid";
    }
}
