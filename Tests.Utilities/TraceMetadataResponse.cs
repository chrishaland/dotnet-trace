namespace Tests.Utilities
{
    public class TraceMetadataResponse
    {
        public string RequestId { get; set; }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }
        public string Sampled { get; set; }
        public string Flags { get; set; }
        public string B3 { get; set; }
    }
}
