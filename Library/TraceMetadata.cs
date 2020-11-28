namespace Haland.DotNetTrace
{
    public class TraceMetadata
    {
        public string RequestId { get; internal set; }
        public string TraceId { get; internal set; }
        public string SpanId { get; internal set; }
        public string ParentSpanId { get; internal set; }
        public string Sampled { get; internal set; }
        public string Flags { get; internal set; }
        public string B3 { get; internal set; }
    }
}
