namespace SocialMedia.Users.Domain.Services;

public interface IPasswordHashingService
{
    string Hash(string plainTextPassword);
}
