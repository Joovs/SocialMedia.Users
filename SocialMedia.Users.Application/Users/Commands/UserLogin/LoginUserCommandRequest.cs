namespace SocialMedia.Users.Application.Users.Commands.UserLogin;

public class LoginUserCommandRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}