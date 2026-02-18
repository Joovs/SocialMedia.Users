using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Users.Domain.Entities.UserEntity.Models;
public class LoginUserModel
{
    [Key]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}