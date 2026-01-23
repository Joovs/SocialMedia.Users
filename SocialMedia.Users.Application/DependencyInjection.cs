using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Users.Application.Commands.Users.Create;
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
        services.AddScoped<ICreateUserCommandValidator, CreateUserCommandValidator>();
        services.AddScoped<IUserFactory, UserFactory>();

        return services;
    }
}
