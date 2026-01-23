using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Users.Infrastructure.InjectionManagers;

namespace SocialMedia.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        RepositoriesManager.AddRepositories(services);

        return services;
    }
}

