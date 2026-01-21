using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Infrastructure.Persistence.Repositories;

namespace SocialMedia.Users.Infrastructure.InjectionManagers;

public static class RepositoriesManager
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IFollowRepository, FollowRepository>();

        return services;
    }
}
