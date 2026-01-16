using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SocialMedia.Users.Application.Commands.UpdateProfile;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Presentation.Modules;

public static class UserModules
{
    private const string BASE_URL = "api/v1/users/";
    public static void AddUserModules(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup(BASE_URL);

        userGroup.MapPut("profile", UpdateProfile);
    }

    private static async Task<IResult> UpdateProfile(
        [FromBody] UpdateProfileCommandRequest request,
        ISender sender,
        CancellationToken cancellationToken
        )
    {
        UpdateProfileCommand command = new UpdateProfileCommand(request);
        Result<UpdateProfileCommandResponse> result = await sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return Results.Problem(
                detail: result.Error?.ErrorMessage ?? result.Message,
                statusCode: result.StatusCode ?? 400,
                title: result.Error?.ErrorCode
            );
        }

        return Results.Created($"{BASE_URL}{result}", result.Value);
    }
}
