using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;
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
        userGroup.MapPost("login", UserLogin)
            .WithName("LoginUser")
            .WithOpenApi(op =>
            {
                op.Summary = "Inicia sesión con email y contraseña";
                op.Description = "Devuelve los datos básicos del usuario y el token JWT si las credenciales son válidas.";
                op.Responses["200"] = new OpenApiResponse
                {
                    Description = "Autenticación exitosa: el usuario está registrado y las credenciales son correctas."
                };
                op.Responses["401"] = new OpenApiResponse
                {
                    Description = "No autorizado: el usuario no está registrado o la contraseña es incorrecta."
                };
                op.Responses["400"] = new OpenApiResponse
                {
                    Description = "Solicitud inválida: faltan datos requeridos."
                };
                return op;
            })
            .Produces<LoginUserCommandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);
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

        return Results.Created($"{BASE_URL}{result.Value!.UserId}", result.Value);
    }

    private static async Task<Results<Ok<LoginUserCommandResponse>, ProblemHttpResult>> UserLogin(
    [FromBody] LoginUserCommandRequest request,
    ISender sender,
    CancellationToken cancellationToken)
    {
        LoginUserCommand command = new LoginUserCommand(request);
        var result = await sender.Send(command, cancellationToken);

        if (!result.IsSuccess || result.Value == null)
        {
            return TypedResults.Problem(
                detail: result.Error?.ErrorMessage ?? result.Message ?? "Error en la autenticación",
                statusCode: result.StatusCode ?? StatusCodes.Status400BadRequest,
                title: result.Error?.ErrorCode ?? "LoginFailed");
        }

        return TypedResults.Ok(result.Value);
    }
}