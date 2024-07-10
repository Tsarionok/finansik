using System.Reflection;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Domain.UseCases.CreateGroup;
using Finansik.Domain.UseCases.DeleteCategory;
using Finansik.Domain.UseCases.GetCategories;
using Finansik.Domain.UseCases.GetGroups;
using Finansik.Domain.UseCases.RenameCategory;
using Finansik.Storage.Entities.Enums;
using Finansik.Storage.Storages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Finansik.Storage.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFinansikStorage(this IServiceCollection services, string connectionString)
    {
        services
            .AddSingleton<IGuidFactory, GuidFactory>()
            .AddScoped<IGetGroupsStorage, GetGroupsStorage>()
            .AddScoped<ICreateGroupStorage, CreateGroupStorage>()
            .AddScoped<IRenameCategoryStorage, RenameCategoryStorage>()
            .AddScoped<IGetCategoriesByGroupIdStorage, GetCategoriesByGroupIdStorage>()
            .AddScoped<ICreateCategoryStorage, CreateCategoryStorage>()
            .AddScoped<IDeleteCategoryStorage, DeleteCategoryStorage>();
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<OperationDirection>();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContextPool<FinansikDbContext>(options => options.UseNpgsql(dataSource));

        services.AddMemoryCache();
        
        services.AddAutoMapper(cfg => cfg.AddMaps(
            Assembly.GetAssembly(typeof(FinansikDbContext))));

        return services;
    }
}