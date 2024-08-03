using System.Diagnostics;
using System.Security.Cryptography;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;

namespace Finansik.API.Monitoring;

public static class LoggingServiceCollectionExtensions
{
    public static IServiceCollection AddApiLogger(this IServiceCollection services, 
        IWebHostEnvironment environment, IConfiguration configuration) =>
        services.AddLogging(b => b.ClearProviders()
            .Configure(options => options.ActivityTrackingOptions = 
                ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId)
            .AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("Application", "Finansik.API")
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .Enrich.With<TracingContextEnricher>()
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .WriteTo.GrafanaLoki(
                        configuration.GetConnectionString("Logs")!,
                        propertiesAsLabels: ["level", "Application", "Environment", "SourceContext"],
                        leavePropertiesIntact: true))
                .WriteTo.Logger(lc => lc.WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
                .CreateLogger()));
    
    private class TracingContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current;
            if (activity is null) return;

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", activity.TraceId));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity.SpanId)); 
        }
    }
}