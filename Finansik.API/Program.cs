using Finansik.API.Middlewares;
using Finansik.Common;
using Finansik.Domain;
using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Domain.UseCases.CreateGroup;
using Finansik.Domain.UseCases.GetCategories;
using Finansik.Domain.UseCases.GetGroups;
using Finansik.Domain.UseCases.RenameCategory;
using Finansik.Storage;
using Finansik.Storage.Storages;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
    .MinimumLevel.Debug()
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
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<OperationDirection>();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddSingleton<IGuidFactory, GuidFactory>();
builder.Services.AddScoped<IIdentityProvider, IdentityProvider>();
builder.Services.AddScoped<IGetGroupsUseCase, GetGroupsUseCase>();
builder.Services.AddScoped<IGetGroupsStorage, GetGroupsStorage>();
builder.Services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
builder.Services.AddScoped<ICreateGroupUseCase, CreateGroupUseCase>();
builder.Services.AddScoped<ICreateGroupStorage, CreateGroupStorage>();
builder.Services.AddScoped<IRenameCategoryUseCase, RenameCategoryUseCase>();
builder.Services.AddScoped<IRenameCategoryStorage, RenameCategoryStorage>();
builder.Services.AddScoped<IGetCategoriesByGroupIdUseCase, GetCategoriesByGroupIdUseCase>();
builder.Services.AddScoped<IGetCategoriesByGroupIdStorage, GetCategoriesByGroupIdStorage>();
builder.Services.AddScoped<ICreateCategoryStorage, CreateCategoryStorage>();
builder.Services.AddScoped<IIntentionResolver, CategoryIntentionResolver>();
builder.Services.AddScoped<IIntentionResolver, GroupIntentionResolver>();
builder.Services.AddScoped<IIntentionManager, IntentionManager>();

builder.Services.AddValidatorsFromAssemblyContaining<IDomain>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<FinansikDbContext>(options => options.UseNpgsql(dataSource));

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