namespace SocialMedia.Users.Application.Users.Commands.UserLogin;

public class LoginUserCommandResponse
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public required string Message { get; set; } = string.Empty;
    public required string Token { get; set; } = string.Empty;
    public required int HttpStatus { get; set; }
}