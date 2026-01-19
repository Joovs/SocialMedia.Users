using Microsoft.AspNetCore.Builder;

namespace SocialMedia.Users.Presentation.Modules;

public class ModulesConfiguration
{
    public static void Configure(WebApplication app)
    {
        app.AddUserModules();
        app.AddFollowModules();
    }
}
