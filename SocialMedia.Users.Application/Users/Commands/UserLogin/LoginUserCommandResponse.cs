namespace SocialMedia.Users.Application.Users.Commands.UserLogin;

public class LoginUserCommandResponse
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Message { get; set; }
    public required string Token { get; set; }
    public required int HttpStatus { get; set; }
}