using System;
using Moq;
using SocialMedia.Users.Application.Users.Commands.UserLogin;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;
using SocialMedia.Users.Domain.Services.JwtServices;
using SocialMedia.Users.Domain.Services.PasswordHashes;

namespace SocialMedia.Users.Test.LoginUserTests;
public class LoginUserHandlerTest
{
    private readonly Mock<IUserRepository> _mockIUserRepository = new();
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<IJwtService> _mockJwtService = new();
    private readonly Mock<IPasswordHasher> _mockPasswordHasher = new();


    [Fact]
    public async Task LoginUserCommandHandler_Success()
    {
        LoginUserCommandRequest request = new()
        {
            Email = "robertchan@example.com",
            Password = "robertC123"
        };

        User user = new()
        {
            Id = Guid.NewGuid(),
            FirstName = "Robert",
            Lastname = "Chan",
            Email = request.Email,
            Password = "hashed-password"
        };
        _mockIUserRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _mockJwtService.Setup(x => x.GenerateToken(user.Email, user.Id)).Returns("jwt-token");
        _mockPasswordHasher.Setup(x => x.VerifyPassword(request.Password, user.Password)).Returns(true);

        var query = new LoginUserCommand(request);
        var queryHandler = new LoginUserCommandHandler(_mockIUserRepository.Object, _mockJwtService.Object, _mockPasswordHasher.Object);
        var result = await queryHandler.Handle(query, _cancellationToken);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}