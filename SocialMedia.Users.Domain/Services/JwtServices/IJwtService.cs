namespace SocialMedia.Users.Domain.Services.JwtServices;

public interface IJwtService
{
    string GenerateToken(string email, Guid userId);
}