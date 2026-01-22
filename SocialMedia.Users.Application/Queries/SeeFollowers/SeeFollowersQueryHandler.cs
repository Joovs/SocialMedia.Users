using SocialMedia.Users.Application.Abstractions.Messaging.Query;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities.FollowsEntity.Models.SeeFollowers;

namespace SocialMedia.Users.Application.Queries.SeeFollowers;

public class SeeFollowersQueryHandler : IQueryHandler<SeeFollowersQuery, SeeFollowersQueryResponse>
{
    private readonly IFollowRepository _followRepository;
    private readonly IUserRepository _userRepository;

    public SeeFollowersQueryHandler(IFollowRepository repository, IUserRepository userRepository)
    {
        _followRepository = repository;
        _userRepository = userRepository;
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

            bool userExists = await _userRepository.UserExists(request.userId, cancellationToken);

            if(!userExists)
            {
                return Result<SeeFollowersQueryResponse>.Failure(404, "UserNotFound", "The user with that ID does not exist");
            }

            List<Follower> followers = await _followRepository .SeeFollowers(request.userId, cancellationToken);

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
