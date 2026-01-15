using System;
using MediatR;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Commands.Example;

public sealed record ExampleCommand (Guid userId) : IRequest<Result<ExampleCommandResponse>>
{
}
