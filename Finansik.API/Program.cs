using Finansik.API.Middlewares;
using Finansik.Domain.DependencyInjection;
using Finansik.Storage.DependencyInjection;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(b => b.ClearProviders()
    .AddSerilog(new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .Enrich.WithProperty("Application", "Finansik.API")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .WriteTo.Logger(lc => lc
        .Filter.ByExcluding(Matching.FromSource("Microsoft"))
        .WriteTo.OpenSearch(
            builder.Configuration.GetConnectionString("Logs"),
            "finansik-logs-{0:yyyy.MM.dd}"))
    .WriteTo.Logger(lc => lc.WriteTo.Console())
    .CreateLogger()));

var connectionString = builder.Configuration.GetConnectionString("Postgres");

builder.Services
    .AddFinansikDomain()
    .AddFinansikStorage(connectionString!);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Used for E2E testing
public abstract partial class Program;