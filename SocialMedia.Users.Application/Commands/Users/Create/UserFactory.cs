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
        string normalizedUsername = command.Username.Trim();
        string normalizedFirstName = command.FirstName.Trim();
        string normalizedLastName = command.LastName.Trim();
        string normalizedEmail = command.Email.Trim().ToLowerInvariant();
        string hashedPassword = _passwordHashingService.Hash(command.Password);
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
