using Microsoft.AspNetCore.Identity;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Services;

namespace SocialMedia.Users.Infrastructure.Security;

public class PasswordHashingService(IPasswordHasher<User> passwordHasher) : IPasswordHashingService
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public string Hash(string plainTextPassword)
    {
        if (string.IsNullOrEmpty(plainTextPassword))
        {
            throw new ArgumentException("Password cannot be empty.", nameof(plainTextPassword));
        }

        return _passwordHasher.HashPassword(new User(), plainTextPassword);
    }
}
