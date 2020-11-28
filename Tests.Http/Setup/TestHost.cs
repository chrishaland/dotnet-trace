using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Haland.DotNetTrace;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
#if NETCOREAPP3_1
using Microsoft.AspNetCore.Http.Json;
#endif

[SetUpFixture]
public class TestHost
{
    private static IHost _host;
    internal static HttpClient SUT;

    internal static JsonSerializerOptions SerializerOptions =>
#if NETCOREAPP3_1
            Microsoft.AspNetCore.Http.Json.JsonOptions.DefaultSerializerOptions;
#else
            new JsonSerializerOptions(JsonSerializerDefaults.Web);
#endif

[OneTimeSetUp]
    public async Task Before()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.ConfigureServices(services =>
                {
                    services.AddTracing();
                });
                webHost.Configure(app =>
                {
                    app.UseTracing();

                    app.Run(async context =>
                    {
                        var traces = context.RequestServices.GetRequiredService<TraceMetadata>();
                        await context.Response.WriteAsJsonAsync(traces);
                    });
                });
            });

        _host = await hostBuilder.StartAsync();
        SUT = _host.GetTestClient();
    }

    [OneTimeTearDown]
    public async Task After()
    {
        await _host.StopAsync();
        SUT = null;
    }
}

// Support for HttpResponse.WriteAsJsonAsync<T> in .NET Core 3.1

#if NETCOREAPP3_1
// https://github.com/dotnet/aspnetcore/blob/912ab2bcb3c6d331279ed97ba7859ff319d611e6/src/Http/Http.Extensions/src/HttpRequestJsonExtensions.cs
#nullable enable
namespace Microsoft.AspNetCore.Http.Json
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Encodings.Web;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static partial class HttpResponseJsonExtensions
    {
        /// <summary>
        /// Write the specified value as JSON to the response body. The response content-type will be set to
        /// <c>application/json; charset=utf-8</c> and the status code set to <c>200</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of object to write.</typeparam>
        /// <param name="response">The response to write JSON to.</param>
        /// <param name="value">The value to write as JSON.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task WriteAsJsonAsync<TValue>(
            this HttpResponse response,
            [AllowNull] TValue value,
            CancellationToken cancellationToken = default)
        {
            return response.WriteAsJsonAsync<TValue>(value, options: null, contentType: null, cancellationToken);
        }

        /// <summary>
        /// Write the specified value as JSON to the response body. The response content-type will be set to
        /// <c>application/json; charset=utf-8</c> and the status code set to <c>200</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of object to write.</typeparam>
        /// <param name="response">The response to write JSON to.</param>
        /// <param name="value">The value to write as JSON.</param>
        /// <param name="options">The serializer options use when serializing the value.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task WriteAsJsonAsync<TValue>(
            this HttpResponse response,
            [AllowNull] TValue value,
            JsonSerializerOptions? options,
            CancellationToken cancellationToken = default)
        {
            return response.WriteAsJsonAsync<TValue>(value, options, contentType: null, cancellationToken);
        }

        /// <summary>
        /// Write the specified value as JSON to the response body. The response content-type will be set to
        /// the specified content-type and the status code set to <c>200</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of object to write.</typeparam>
        /// <param name="response">The response to write JSON to.</param>
        /// <param name="value">The value to write as JSON.</param>
        /// <param name="options">The serializer options use when serializing the value.</param>
        /// <param name="contentType">The content-type to set on the response.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task WriteAsJsonAsync<TValue>(
            this HttpResponse response,
            [AllowNull] TValue value,
            JsonSerializerOptions? options,
            string? contentType,
            CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            options ??= ResolveSerializerOptions(response.HttpContext);

            response.ContentType = contentType ?? "application/json; charset=utf-8";
            response.StatusCode = StatusCodes.Status200OK;
            return JsonSerializer.SerializeAsync<TValue>(response.Body, value!, options, cancellationToken);
        }

        /// <summary>
        /// Write the specified value as JSON to the response body. The response content-type will be set to
        /// <c>application/json; charset=utf-8</c> and the status code set to <c>200</c>.
        /// </summary>
        /// <param name="response">The response to write JSON to.</param>
        /// <param name="value">The value to write as JSON.</param>
        /// <param name="type">The type of object to write.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task WriteAsJsonAsync(
            this HttpResponse response,
            object? value,
            Type type,
            CancellationToken cancellationToken = default)
        {
            return response.WriteAsJsonAsync(value, type, options: null, contentType: null, cancellationToken);
        }

        /// <summary>
        /// Write the specified value as JSON to the response body. The response content-type will be set to
        /// <c>application/json; charset=utf-8</c> and the status code set to <c>200</c>.
        /// </summary>
        /// <param name="response">The response to write JSON to.</param>
        /// <param name="value">The value to write as JSON.</param>
        /// <param name="type">The type of object to write.</param>
        /// <param name="options">The serializer options use when serializing the value.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task WriteAsJsonAsync(
            this HttpResponse response,
            object? value,
            Type type,
            JsonSerializerOptions? options,
            CancellationToken cancellationToken = default)
        {
            return response.WriteAsJsonAsync(value, type, options, contentType: null, cancellationToken);
        }

        /// <summary>
        /// Write the specified value as JSON to the response body. The response content-type will be set to
        /// the specified content-type and the status code set to <c>200</c>.
        /// </summary>
        /// <param name="response">The response to write JSON to.</param>
        /// <param name="value">The value to write as JSON.</param>
        /// <param name="type">The type of object to write.</param>
        /// <param name="options">The serializer options use when serializing the value.</param>
        /// <param name="contentType">The content-type to set on the response.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task WriteAsJsonAsync(
            this HttpResponse response,
            object? value,
            Type type,
            JsonSerializerOptions? options,
            string? contentType,
            CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            options ??= ResolveSerializerOptions(response.HttpContext);

            response.ContentType = contentType ?? "application/json; charset=utf-8";
            response.StatusCode = StatusCodes.Status200OK;
            return JsonSerializer.SerializeAsync(response.Body, value, type, options, cancellationToken);
        }

        private static JsonSerializerOptions ResolveSerializerOptions(HttpContext httpContext)
        {
            // Attempt to resolve options from DI then fallback to default options
            return httpContext.RequestServices?.GetService<IOptions<JsonOptions>>()?.Value?.SerializerOptions ?? JsonOptions.DefaultSerializerOptions;
        }
    }

    public class JsonOptions
    {
        // https://github.com/dotnet/docs/issues/21067

        internal static readonly JsonSerializerOptions DefaultSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            //NumberHandling = Serialization.JsonNumberHandling.AllowReadingFromString,

            // Web defaults don't use the relex JSON escaping encoder.
            //
            // Because these options are for producing content that is written directly to the request
            // (and not embedded in an HTML page for example), we can use UnsafeRelaxedJsonEscaping.
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        // Use a copy so the defaults are not modified.
        public JsonSerializerOptions SerializerOptions { get; } = DefaultSerializerOptions;
    }
}
#endif
