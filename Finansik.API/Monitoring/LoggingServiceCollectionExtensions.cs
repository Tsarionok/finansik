using Serilog;
using Serilog.Filters;

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
                    .WriteTo.OpenSearch(
                        configuration.GetConnectionString("Logs"),
                        "finansik-logs-{0:yyyy.MM.dd}"))
                .WriteTo.Logger(lc => lc.WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
                .CreateLogger()));
}