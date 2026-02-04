using MediatR;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Commands.Users.Create;

public sealed record CreateUserCommand(CreateUserCommandRequest Request) : IRequest<Result<CreateUserCommandResponse>>;
