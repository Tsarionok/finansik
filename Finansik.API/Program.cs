using System.Reflection;
using AutoMapper;
using Finansik.API.Authentication;
using Finansik.API.DependencyInjection;
using Finansik.API.Middlewares;
using Finansik.API.Models.Mapping;
using Finansik.Domain.Authentication;
using Finansik.Domain.DependencyInjection;
using Finansik.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiLogger(builder.Environment, builder.Configuration);
builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication").Bind);
builder.Services.AddScoped<IAuthTokenStorage, AuthTokenStorage>();
    
builder.Services
    .AddFinansikDomain()
    .AddFinansikStorage(builder.Configuration.GetConnectionString("Postgres")!);

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(ApiMappingProfile)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Used for E2E testing
public abstract partial class Program;