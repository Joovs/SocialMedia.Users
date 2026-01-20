using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Infrastructure.Persistence.Repositories;

namespace SocialMedia.Users.Infrastructure.InjectionManagers;

public static class RepositoriesManager
{
    public static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFollowRepository, FollowRepository>();
    }
}

