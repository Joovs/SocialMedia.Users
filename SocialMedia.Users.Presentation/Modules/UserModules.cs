using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SocialMedia.Users.Application.Commands.Users.Create;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Presentation.Contracts.Users;

namespace SocialMedia.Users.Presentation.Modules;

public static class UserModules
{
    private const string BASE_URL = "api/v1/users/";
    public static void AddUserModules(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup(BASE_URL);

        userGroup.MapPost(string.Empty, CreateUser)
                 .WithName("CreateUser")
                 .WithTags("Users")
                 .Produces<CreateUserCommandResponse>(StatusCodes.Status201Created)
                 .ProducesProblem(StatusCodes.Status400BadRequest)
                 .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> CreateUser(
        [FromBody] CreateUserRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var commandRequest = new CreateUserCommandRequest(
            request.Username,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);
        CreateUserCommand command = new CreateUserCommand(commandRequest);
        Result<CreateUserCommandResponse> result = await sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return Results.Problem(
                detail: result.Error?.ErrorMessage ?? result.Message,
                statusCode: result.StatusCode ?? 400,
                title: result.Error?.ErrorCode
            );
        }

        if (result.Value is null)
        {
            return Results.Problem(
                detail: "The user was created but the payload could not be generated.",
                statusCode: 500,
                title: "UserPayloadMissing"
            );
        }

        return Results.Created($"{BASE_URL}{result.Value.Id}", result.Value);
    }
}
