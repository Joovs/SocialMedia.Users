namespace SocialMedia.Users.Application.Commands.Users.Create;

public sealed record CreateUserCommandRequest(
    string Username,
    string FirstName,
    string LastName,
    string Email,
    string Password);
