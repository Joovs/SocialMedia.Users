using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Users.Infrastructure.InjectionManagers;
using SocialMedia.Users.Application.Abstractions;
using SocialMedia.Users.Infrastructure.Persistence;
using SocialMedia.Users.Infrastructure.Persistence.Context;

namespace SocialMedia.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        RepositoriesManager.AddRepositories(services);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
