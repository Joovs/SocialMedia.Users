using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SocialMedia.Users.Application.Queries.SeeFollowers;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Presentation.Modules;

public static class FollowModules
{
    private const string BASE_URL = "api/v1/follows/";

    public static void AddFollowModules(this IEndpointRouteBuilder app)
    {
        var followGroup = app.MapGroup(BASE_URL);

        followGroup.MapGet("followers/{userID}", SeeFollowers);
    }

    private static async Task<IResult> SeeFollowers(
        [FromRoute] Guid userID,
        ISender sender,
        CancellationToken cancellationToken
        )
    {
        SeeFollowersQuery query = new SeeFollowersQuery(userID);
        Result<SeeFollowersQueryResponse> result = await sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return Results.Problem(
                detail: result.Error?.ErrorMessage ?? result.Message,
                statusCode: result.StatusCode ?? 400,
                title: result.Error?.ErrorCode
            );
        }

        return Results.Created($"{BASE_URL}{result.Value.UserId}", result.Value);
    }
}
