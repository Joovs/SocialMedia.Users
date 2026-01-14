namespace SocialMedia.Users.Domain.Entities.UserEntity.Models.UpdateProfile;

public class UpdateProfileModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
