using MediatR;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Commands.UpdateProfile;

public sealed record UpdateProfileCommand (UpdateProfileCommandRequest request) : IRequest<Result<UpdateProfileCommandResponse>>
{
}
