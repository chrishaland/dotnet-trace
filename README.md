# dotnet-trace

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Haland.DotNetTrace?color=informational)](https://www.nuget.org/packages/Haland.DotNetTrace/)

This project aims to allow for [distributed tracing](https://istio.io/latest/faq/distributed-tracing/) through .NET applications in Kubernetes by propagating the [B3 trace headers](https://github.com/openzipkin/b3-propagation).

## Getting started

Add reference to the nuget package:

> dotnet nuget add Haland.DotNetTrace

In your `Startup.cs` file (or where ever you've placed your service collection registration and middleware pipeline setup):

Add import
```
using Haland.DotNetTrace;
```

Register tracing services
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddTracing();
}
```

Add tracing middleware to the middleware pipeline
```
public void Configure(IApplicationBuilder app)
{
    app.UseTracing();
}
```
