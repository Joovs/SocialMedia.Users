using MediatR;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Users.Commands.UserLogin;

public sealed record LoginUserCommand(LoginUserCommandRequest request) : IRequest<Result<LoginUserCommandResponse>>;