using MediatR;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Commands.Users.Create;

public sealed record CreateUserCommand(string Username, string FirstName, string LastName, string Email, string Password) : IRequest<Result<CreateUserCommandResponse>>;
