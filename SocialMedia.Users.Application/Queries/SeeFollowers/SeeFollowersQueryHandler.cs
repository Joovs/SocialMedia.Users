using SocialMedia.Users.Application.Abstractions.Messaging.Query;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities.FollowsEntity.Models.SeeFollowers;
using SocialMedia.Users.Domain.Entities.FollowsEntity.Repositories;

namespace SocialMedia.Users.Application.Queries.SeeFollowers;

public class SeeFollowersQueryHandler : IQueryHandler<SeeFollowersQuery, SeeFollowersQueryResponse>
{
    private readonly IFollowRepository _repository;

    public SeeFollowersQueryHandler(IFollowRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<SeeFollowersQueryResponse>> Handle(SeeFollowersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null)
            {
                return Result<SeeFollowersQueryResponse>.Failure(400, "AgrgumentException", "User Id is required");
            }

            if(request.userId == Guid.Empty)
            {
                return Result<SeeFollowersQueryResponse>.Failure(400, "AgrgumentException", "Empty User Id");
            }

            bool userExists = await _repository.UserExists(request.userId, cancellationToken);

            if(!userExists)
            {
                return Result<SeeFollowersQueryResponse>.Failure(400, "ArgumentExeption", "InvalidID");
            }

            List<Follower> followers = await _repository.SeeFollowers(request.userId, cancellationToken);

            SeeFollowersQueryResponse response = new SeeFollowersQueryResponse
            {
                UserId = request.userId,
                Followers = followers,
            };

            return Result<SeeFollowersQueryResponse>.Success(response);

        }
        catch (Exception ex)
        {
            return Result<SeeFollowersQueryResponse>.Failure(500, "InternalServerError", ex.Message);
        }
    }
}
