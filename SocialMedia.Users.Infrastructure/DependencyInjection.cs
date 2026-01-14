using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Services;
using SocialMedia.Users.Infrastructure.InjectionManagers;
using SocialMedia.Users.Infrastructure.Security;

namespace SocialMedia.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IPasswordHashingService, PasswordHashingService>();
        RepositoriesManager.AddRepositories(services);

        return services;
    }
}
