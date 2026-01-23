using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Commands.Users.Create;

public interface ICreateUserCommandValidator
{
    Result<CreateUserCommandResponse>? Validate(CreateUserCommand command);
}
