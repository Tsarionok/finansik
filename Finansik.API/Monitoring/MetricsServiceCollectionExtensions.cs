using OpenTelemetry.Metrics;

namespace Finansik.API.Monitoring;

public static class MetricsServiceCollectionExtensions
{
    public static IServiceCollection AddApiMetrics(this IServiceCollection services)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddMeter("Finansik.Domain")
                .AddAspNetCoreInstrumentation()
                .AddPrometheusExporter()
                .AddView("http.server.request.duration", new ExplicitBucketHistogramConfiguration
                {
                    Boundaries = [0, 0.05, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10]
                }));

        return services;
    }
}