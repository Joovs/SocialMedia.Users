using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Domain.Services.JwtServices;
using SocialMedia.Users.Domain.Services.PasswordHashes;
using SocialMedia.Users.Infrastructure.Persistence.Repositories;
using SocialMedia.Users.Infrastructure.Services;
using SocialMedia.Users.Infrastructure.Services.PasswordHasher;

namespace SocialMedia.Users.Infrastructure.InjectionManagers;

public static class RepositoriesManager
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
