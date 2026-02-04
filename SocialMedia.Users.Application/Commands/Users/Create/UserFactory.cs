using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Services;

namespace SocialMedia.Users.Application.Commands.Users.Create;

public class UserFactory(
    IPasswordHashingService passwordHashingService,
    IDateTimeProvider dateTimeProvider) : IUserFactory
{
    private readonly IPasswordHashingService _passwordHashingService = passwordHashingService;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public User CreateNew(CreateUserCommand command)
    {
        string normalizedUsername = command.Request.Username.Trim();
        string normalizedFirstName = command.Request.FirstName.Trim();
        string normalizedLastName = command.Request.LastName.Trim();
        string normalizedEmail = command.Request.Email.Trim().ToLowerInvariant();
        string hashedPassword = _passwordHashingService.Hash(command.Request.Password);
        DateTime localNow = _dateTimeProvider.GetLocalTime();

        return new User
        {
            Id = Guid.NewGuid(),
            Username = normalizedUsername,
            FirstName = normalizedFirstName,
            LastName = normalizedLastName,
            Email = normalizedEmail,
            Password = hashedPassword,
            CreatedAt = localNow,
            UpdateAt = localNow
        };
    }
}
