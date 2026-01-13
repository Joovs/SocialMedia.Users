using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SocialMedia.Users.Application.Commands.Example;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Application.Users.Commands.UserLogin;

namespace SocialMedia.Users.Presentation.Modules;

public static class UserModules
{
    private const string BASE_URL = "api/v1/users/";
    public static void AddUserModules(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup(BASE_URL);

        userGroup.MapPut("example/{userId}", ExampleUsers);
        userGroup.MapPost("login", UserLogin);
    }

    private static async Task<IResult> ExampleUsers(
        [FromRoute] Guid userID,
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

    private static async Task<IResult> UserLogin(
    [FromBody] LoginUserCommandRequest request,
    ISender sender,
    CancellationToken cancellationToken)
    {
        LoginUserCommand command = new LoginUserCommand(request);
        var result = await sender.Send(command, cancellationToken);

        if (result.Value == null)
            return Results.Content("Usuario bloqueado o inactivo");

        return Results.Ok(result.Value);
    }
}
