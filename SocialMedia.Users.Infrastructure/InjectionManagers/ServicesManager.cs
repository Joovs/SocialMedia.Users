
﻿using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Users.Domain.Services.Hasher;
using SocialMedia.Users.Infrastructure.Services.Hasher;
using SocialMedia.Users.Domain.Services.JwtServices;
using SocialMedia.Users.Domain.Services.PasswordHashes;
using SocialMedia.Users.Infrastructure.Services;
using SocialMedia.Users.Infrastructure.Services.PasswordHasher;

namespace SocialMedia.Users.Infrastructure.InjectionManagers;

public static class ServicesManager
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IHasher, Hasher>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
