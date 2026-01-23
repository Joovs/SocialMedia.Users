using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SocialMedia.Users.Application.Commands.Example;
using SocialMedia.Users.Application.Commands.Follow.FollowUserCommand;
using SocialMedia.Users.Application.Commands.Follow.UnfollowUserCommand;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Presentation.Modules;

public static class UserModules
{
    private const string BASE_URL = "api/v1/users/";
    public static void AddUserModules(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup(BASE_URL);

        userGroup.MapPut("example/{userId}", ExampleUsers);
        userGroup.MapPost("follow", FollowUser);
        userGroup.MapDelete("unfollow", UnfollowUser);
    }

    private static async Task<IResult> ExampleUsers(
        [FromRoute] int userID,
        ISender sender,
        CancellationToken cancellationToken
        )
    {
        ExampleCommand command = new ExampleCommand(userID);
        Result<ExampleCommandResponse> result = await sender.Send(command, cancellationToken);

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

    private static async Task<IResult> FollowUser(
        [FromBody] FollowUserCommand command,
        ISender sender,
        CancellationToken cancellationToken
        )
    {
        try
        {
            FollowCommandResponse result = await sender.Send(command, cancellationToken);
            return Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static async Task<IResult> UnfollowUser(
        [FromBody] UnfollowUserCommand command,
        ISender sender,
        CancellationToken cancellationToken
        )
    {
        try
        {
            UnfollowCommandResponse result = await sender.Send(command, cancellationToken);
            return Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }
}

