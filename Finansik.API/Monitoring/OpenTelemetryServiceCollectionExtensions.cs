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
                .ConfigureResource(r => r.AddService("TFA"))
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation(cfg => cfg.SetDbStatementForText = true)
                .AddSource("Finansik.Domain")
                .AddJaegerExporter(
                    options => options.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!)));

        return services;
    }
}