using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.UseCases;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Domain.UseCases.CreateGroup;
using Finansik.Domain.UseCases.GetCategories;
using Finansik.Domain.UseCases.GetGroup;
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
            //.AddScopedUseCase<GetGroupCommand, GetGroupResult, GetGroupUseCase>()
            .AddScopedUseCase<GetGroupUseCase>()
            .AddScoped<IGetGroupsUseCase, GetGroupsUseCase>()
            .AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>()
            .AddScoped<ICreateGroupUseCase, CreateGroupUseCase>()
            .AddScoped<IRenameCategoryUseCase, RenameCategoryUseCase>()
            .AddScoped<IGetCategoriesByGroupIdUseCase, GetCategoriesByGroupIdUseCase>();

        // TODO: try to remove IFinansikDomain
        services.AddValidatorsFromAssemblyContaining<IFinansikDomain>(includeInternalTypes: true);
        
        return services;
    }
    
    [Experimental]
    private static IServiceCollection AddScopedUseCase<TImplementation>(this IServiceCollection services)
        where TImplementation : class, IUseCase
    {
        var implType = typeof(TImplementation);
        var genericArguments = implType.GetInterfaces().First().GetGenericArguments();
        if (genericArguments.Length == 0)
        {
            throw new RegisterUseCaseException();
            return services.AddScoped<IUseCase, TImplementation>();
        }

        Type[] typeArgs = [genericArguments[0], genericArguments[1]];
        var constructed = typeof(IUseCase<,>).MakeGenericType(typeArgs);

        return services.AddScoped(constructed, typeof(TImplementation));
    }
}