using MediatR;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Users.Queries.SeeProfile;
public sealed record SeeProfileQuery(Guid userId) : IRequest<Result<SeeProfileQueryResponse>>;