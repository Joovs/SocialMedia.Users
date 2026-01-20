using MediatR;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Queries.GetUserFollowing;

public sealed record GetUserFollowingQuery(Guid UserId)
    : IRequest<Result<GetUserFollowingResponse>>;

