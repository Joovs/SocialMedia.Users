using MediatR;
using SocialMedia.Users.Application.Queries.GetUserFollowing;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities.FollowEntity;

public class GetUserFollowingQueryHandler
    : IRequestHandler<GetUserFollowingQuery, Result<GetUserFollowingQueryResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IFollowRepository _followRepository;

    public GetUserFollowingQueryHandler(
        IUserRepository userRepository,
        IFollowRepository followRepository)
    {
        _userRepository = userRepository;
        _followRepository = followRepository;
    }

    public async Task<Result<GetUserFollowingQueryResponse>> Handle(
        GetUserFollowingQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            if (query.UserId == Guid.Empty)
            {
                return Result<GetUserFollowingQueryResponse>.Failure(
                    new Error("INVALID_USER_ID", "UserId is invalid")
                );
            }

            if (!await _userRepository.ExistsAsync(query.UserId, cancellationToken))
            {
                return Result<GetUserFollowingQueryResponse>.Failure(
                    new Error("USER_NOT_FOUND", "User does not exist")
                );
            }

            List<UserFollow> following = await _followRepository.GetFollowingAsync(query.UserId, cancellationToken);

            GetUserFollowingQueryResponse response = new()
            {
                Following = following
            };

            return Result<GetUserFollowingQueryResponse>.Success(response);
        }
        catch (Exception ex) { 
        
            return Result<GetUserFollowingQueryResponse>.Failure(
                500,
                "INTERNAL_SERVER_ERROR",
                $"An error occurred while retrieving following users: {ex.Message}"
            );
        }
    }
}
