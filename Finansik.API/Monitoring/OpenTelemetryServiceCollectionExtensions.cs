using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Finansik.API.Monitoring;

public static class OpenTelemetryServiceCollectionExtensions
{
    public static IServiceCollection AddApiMetrics(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddMeter("Finansik.Domain")
                .AddAspNetCoreInstrumentation()
                .AddPrometheusExporter())
            .WithTracing(builder => builder
                .ConfigureResource(r => r.AddService("Finansik"))
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.Filter += context =>
                        !context.Request.Path.Value!.Contains("metrics", StringComparison.InvariantCultureIgnoreCase) &&
                        !context.Request.Path.Value!.Contains("swagger", StringComparison.InvariantCultureIgnoreCase);
                    options.EnrichWithHttpResponse = (activity, response) =>
                        activity.AddTag("error", response.StatusCode >= 400);
                })
                .AddEntityFrameworkCoreInstrumentation(cfg => cfg.SetDbStatementForText = true)
                .AddSource("Finansik.Domain")
                .AddJaegerExporter(
                    options => options.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!)));

        return services;
    }
}