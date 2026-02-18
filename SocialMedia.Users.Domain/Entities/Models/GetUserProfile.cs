using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Users.Domain.Entities.Models;
public class GetUserProfile
{
    [Key]
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }
}