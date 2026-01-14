using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SocialMedia.Users.Application.Commands.Users.Create;
using SocialMedia.Users.Application.Commands.Example;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Presentation.Contracts.Users;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
                 .ProducesValidationProblem()
                 .ProducesProblem(StatusCodes.Status400BadRequest)
                 .ProducesProblem(StatusCodes.Status500InternalServerError);
        userGroup.MapPut("example/{userId}", ExampleUsers);
    }

    private static async Task<IResult> CreateUser(
        [FromBody] CreateUserRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        IResult? validationResult = ValidateRequest(request);
        if (validationResult is not null)
        {
            return validationResult;
        }

        CreateUserCommand command = new CreateUserCommand(request.Username, request.FistName, request.Lastname, request.Email, request.Password);
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

        return Results.Created($"{BASE_URL}{result.Value.UserId}", result.Value);
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

        if (result.Value is null)
        {
            return Results.Problem(
                detail: "The user payload could not be generated.",
                statusCode: 500,
                title: "UserPayloadMissing"
            );
        }

        return Results.Created($"{BASE_URL}{result.Value.UserId}", result.Value);
    }

    private static IResult? ValidateRequest(CreateUserRequest request)
    {
        List<ValidationResult> validationResults = new();
        ValidationContext validationContext = new ValidationContext(request);
        bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

        if (isValid)
        {
            return null;
        }

        Dictionary<string, string[]> errors = validationResults
            .GroupBy(r => r.MemberNames.FirstOrDefault() ?? string.Empty)
            .ToDictionary(
                g => string.IsNullOrEmpty(g.Key) ? "Request" : g.Key,
                g => g.Select(r => r.ErrorMessage ?? "Invalid value.").ToArray()
            );

        return Results.ValidationProblem(errors);
    }
}
