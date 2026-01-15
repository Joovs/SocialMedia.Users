using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SocialMedia.Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication (this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Register command handlers that are used directly by controllers
        services.AddScoped<Commands.Follow.FollowUserCommandHandler>();
        services.AddScoped<Commands.Follow.UnfollowUserCommandHandler>();

        return services;
    }
}
