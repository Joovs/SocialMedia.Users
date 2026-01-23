using SocialMedia.Users.Domain.Entities.UserEntity;

namespace SocialMedia.Users.Application.Commands.Users.Create;

public interface IUserFactory
{
    User CreateNew(CreateUserCommand command);
}
