using Microsoft.AspNetCore.Mvc;
using SocialMedia.Users.Application.Commands.Follow;
using System;
using System.Threading.Tasks;
namespace SocialMedia.Users.Presentation.Modules.Follow
{
    [ApiController]
    [Route("user/{userId}/follow")]
    public class FollowController : ControllerBase
    {
        private readonly FollowUserCommandHandler _followHandler;
    private readonly UnfollowUserCommandHandler _unfollowHandler;

    public FollowController(
        FollowUserCommandHandler followHandler,
        UnfollowUserCommandHandler unfollowHandler)
    {
        _followHandler = followHandler;
        _unfollowHandler = unfollowHandler;
    }

    [HttpPost("{followingUserId}")]
    public async Task<IActionResult> Follow(Guid userId, Guid followingUserId)
    {
        var result = await _followHandler.Handle(new FollowUserCommand
        {
            FollowerUserId = userId,
            FollowingUserId = followingUserId
        });

        return Ok(result);
    }

    [HttpDelete("{followingUserId}")]
    public async Task<IActionResult> Unfollow(Guid userId, Guid followingUserId)
    {
        var result = await _unfollowHandler.Handle(new UnfollowUserCommand
        {
            FollowerUserId = userId,
            FollowingUserId = followingUserId
        });

        return Ok(result);
    }
    }
}