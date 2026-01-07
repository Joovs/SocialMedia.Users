using MediatR;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Commands.Example;

public sealed record ExampleCommand (int userId) : IRequest<Result<ExampleCommandResponse>>
{
}
