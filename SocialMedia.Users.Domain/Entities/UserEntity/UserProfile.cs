using SocialMedia.Users.Domain.Entities.Models;

namespace SocialMedia.Users.Domain.Entities;

public class UserProfile
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public List<GetPosts>? Posted { get; set; }
}