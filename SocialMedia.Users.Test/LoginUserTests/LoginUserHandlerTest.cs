using Moq;
using SocialMedia.Users.Application.Users.Commands.UserLogin;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;
using SocialMedia.Users.Domain.Services.JwtServices;

namespace SocialMedia.Users.Test.LoginUserTests;
public class LoginUserHandlerTest
{
    private readonly Mock<IUserRepository> _mockIUserRepository = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly IJwtService _jwtService;


    [Fact]
    public async Task LoginUserCommandHandler_Success()
    {
        LoginUserCommandRequest request = new()
        {
            Email = "robertchan@example.com",
            Password = "robertC123"
        };

        User user = new();
        //LoginUserCommandResponse response = new LoginUserCommandResponse();
        _mockIUserRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);

        var query = new LoginUserCommand(request);
        var queryHandler = new LoginUserCommandHandler(_mockIUserRepository.Object, _jwtService);
        var result = await queryHandler.Handle(query, _cancellationToken);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}