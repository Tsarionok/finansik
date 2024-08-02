using Finansik.Domain.Authentication;
using Finansik.Domain.Authentication.Cryptography;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Category;
using Finansik.Domain.Authorization.Group;
using Finansik.Domain.Monitoring;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Finansik.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFinansikDomain(this IServiceCollection services)
    {
        // TODO: unshackle from DomainMetrics for add MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DomainMetrics>());
        
        // TODO: replace factory/provider/managers/resolvers dependencies to another module
        services
            .AddScoped<IIdentityProvider, IdentityProvider>()
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<ISymmetricDecryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<ISymmetricEncryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<IIntentionResolver, CategoryIntentionResolver>()
            .AddScoped<IIntentionResolver, GroupIntentionResolver>();
        
        // TODO: unshackle from DomainMetrics for add validators
        services.AddValidatorsFromAssemblyContaining<DomainMetrics>(includeInternalTypes: true);

        services.AddSingleton<DomainMetrics>();
        
        return services;
    }
}