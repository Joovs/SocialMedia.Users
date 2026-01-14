using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Users.Domain.Services.Hasher;
using SocialMedia.Users.Infrastructure.Services.Hasher;

namespace SocialMedia.Users.Infrastructure.InjectionManagers;

public static class ServicesManager
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IHasher, Hasher>();
        return services;
    }
}
