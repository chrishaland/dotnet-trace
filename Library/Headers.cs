namespace Haland.DotNetTrace
{
    internal static class Headers
    {
        internal const string RequestId = "x-request-id";
        internal const string TraceId = "x-b3-traceid";
        internal const string SpanId = "x-b3-spanid";
        internal const string ParentSpanId = "x-b3-parentspanid";
        internal const string Sampled = "x-b3-sampled";
        internal const string Flags = "x-b3-flags";
        internal const string B3 = "b3";
    }
}
