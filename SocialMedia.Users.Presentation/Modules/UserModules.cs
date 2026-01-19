using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Presentation.Modules;

public static class UserModules
{
    private const string BASE_URL = "api/v1/users/";
    public static void AddUserModules(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup(BASE_URL);

    }


}
