using Serilog;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;

namespace Finansik.API.Monitoring;

public static class LoggingServiceCollectionExtensions
{
    public static IServiceCollection AddApiLogger(this IServiceCollection services, 
        IWebHostEnvironment environment, IConfiguration configuration) =>
        services.AddLogging(b => b.ClearProviders()
            .AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("Application", "Finansik.API")
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .WriteTo.GrafanaLoki(
                        configuration.GetConnectionString("Logs")!,
                        propertiesAsLabels: ["level", "Application", "Environment", "SourceContext"],
                        leavePropertiesIntact: true))
                .WriteTo.Logger(lc => lc.WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
                .CreateLogger()));
}