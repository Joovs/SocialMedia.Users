using SocialMedia.Users.Application.Abstractions.Messaging.Query;

namespace SocialMedia.Users.Application.Queries.SeeFollowers;

public sealed record SeeFollowersQuery (Guid userId) : IQuery<SeeFollowersQueryResponse>
{
}
