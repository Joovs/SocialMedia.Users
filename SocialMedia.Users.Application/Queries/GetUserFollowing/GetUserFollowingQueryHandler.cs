using MediatR;
using SocialMedia.Users.Application.Queries.GetUserFollowing;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Application.Shared;

public class GetUserFollowingQueryHandler
    : IRequestHandler<GetUserFollowingQuery, Result<GetUserFollowingResponse>>
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

    public async Task<Result<GetUserFollowingResponse>> Handle(
        GetUserFollowingQuery query,
        CancellationToken cancellationToken)
    {
        if (query.UserId == Guid.Empty)
        {
            return Result<GetUserFollowingResponse>.Failure(
                new Error("INVALID_USER_ID", "UserId is invalid")
            );
        }

        if (!await _userRepository.ExistsAsync(query.UserId))
        {
            return Result<GetUserFollowingResponse>.Failure(
                new Error("USER_NOT_FOUND", "User does not exist")
            );
        }

        var following = await _followRepository.GetFollowingAsync(query.UserId);

        var response = new GetUserFollowingResponse
        {
            Following = following
        };

        return Result<GetUserFollowingResponse>.Success(response);
    }
}
