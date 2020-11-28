using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Http;

namespace Haland.DotNetTrace
{
    public class TraceHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
    {
        public TraceHttpMessageHandlerBuilder(IServiceProvider services)
        {
            Services = services;
            PrimaryHandler = new HttpClientHandler();
            AdditionalHandlers = new List<DelegatingHandler> { new TraceHttpMessageHandler(Services) };
        }

        public override string Name { get; set; }
        public override IServiceProvider Services { get; }
        public override HttpMessageHandler PrimaryHandler { get; set; }
        public override IList<DelegatingHandler> AdditionalHandlers { get; }

        public override HttpMessageHandler Build()
        {
            return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
        }
    }
}
