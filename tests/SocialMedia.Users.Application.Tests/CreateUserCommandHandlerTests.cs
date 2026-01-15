using FluentAssertions;
using Moq;
using SocialMedia.Users.Application.Commands.Users.Create;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;
using SocialMedia.Users.Domain.Services;

namespace SocialMedia.Users.Application.Tests;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _repositoryMock = new();
    private readonly Mock<IPasswordHashingService> _passwordHashingServiceMock = new();
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _handler = new CreateUserCommandHandler(_repositoryMock.Object, _passwordHashingServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenUsernameIsMissing()
    {
        CreateUserCommand command = new CreateUserCommand(string.Empty, "John", "Doe", "john@example.com", "password123!");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.ErrorCode.Should().Be("InvalidUsername");
        _repositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenPayloadIsValid()
    {
        CreateUserCommand command = new CreateUserCommand("  DemoUser  ", " John ", " Doe ", "TEST@MAIL.com", "password123!");
        _passwordHashingServiceMock.Setup(p => p.Hash(command.Password)).Returns("hashed-value");

        User? capturedUser = null;
        _repositoryMock.Setup(r => r.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((user, _) => capturedUser = user)
            .ReturnsAsync((User user, CancellationToken _) =>
            {
                user.Id = Guid.NewGuid();
                return user;
            });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Username.Should().Be("DemoUser");
        result.Value!.Email.Should().Be("test@mail.com");

        capturedUser.Should().NotBeNull();
        capturedUser!.Password.Should().Be("hashed-value");
        capturedUser.CreatedAt.Should().BeCloseTo(capturedUser.UpdateAt, TimeSpan.FromSeconds(1));
        capturedUser.Username.Should().Be("DemoUser");
        capturedUser.Email.Should().Be("test@mail.com");
    }
}
