namespace SocialMedia.Users.Application.Commands.UpdateProfile;

public class UpdateProfileCommandRequest
{
    public required int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
