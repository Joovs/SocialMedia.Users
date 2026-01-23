using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SocialMedia.Users.Application.Queries.GetUserFollowing;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Presentation.Modules;

public static class UserFollowingModule
{
    private const string BASE_URL = "api/v1/users/";

    public static void AddUserFollowingModule(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup(BASE_URL);

        group.MapGet("{userId:guid}/following", GetUserFollowing);
    }

    private static async Task<IResult> GetUserFollowing(
        [FromRoute] Guid userId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
            return Results.BadRequest("UserId is required");

        GetUserFollowingQuery query = new(userId);
        Result<GetUserFollowingQueryResponse> result = await sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Error?.ErrorCode switch
            {
                "USER_NOT_FOUND" => Results.NotFound(result.Error),
                _ => Results.BadRequest(result.Error)
            };
        }

        return Results.Ok(result.Value);
    }
}
