using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Users.Presentation.Contracts.Users;

public sealed record CreateUserRequest(
    [property: Required, StringLength(50, MinimumLength = 3)]
    string Username,
    [property: Required, StringLength(80, MinimumLength = 2)]
    string Lastname,
    [property: Required, EmailAddress, StringLength(120)]
    string Email,
    [property: Required, StringLength(128, MinimumLength = 8)]
    string Password);
