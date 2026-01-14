namespace SocialMedia.Users.Application.Commands.Users.Create;

public class CreateUserCommandResponse
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FistName { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
