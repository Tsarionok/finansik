using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Domain.UseCases.CreateGroup;
using Finansik.Domain.UseCases.GetCategories;
using Finansik.Domain.UseCases.GetGroups;
using Finansik.Domain.UseCases.RenameCategory;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Finansik.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFinansikDomain(this IServiceCollection services)
    {
        // TODO: replace factory/provider/managers/resolvers dependencies to another module
        services
            .AddScoped<IIdentityProvider, IdentityProvider>()
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIntentionResolver, CategoryIntentionResolver>()
            .AddScoped<IIntentionResolver, GroupIntentionResolver>();
            
        services
            .AddScoped<IGetGroupsUseCase, GetGroupsUseCase>()
            .AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>()
            .AddScoped<ICreateGroupUseCase, CreateGroupUseCase>()
            .AddScoped<IRenameCategoryUseCase, RenameCategoryUseCase>()
            .AddScoped<IGetCategoriesByGroupIdUseCase, GetCategoriesByGroupIdUseCase>();

        // TODO: try to remove IFinansikDomain
        services.AddValidatorsFromAssemblyContaining<IFinansikDomain>(includeInternalTypes: true);
        
        return services;
    }
}