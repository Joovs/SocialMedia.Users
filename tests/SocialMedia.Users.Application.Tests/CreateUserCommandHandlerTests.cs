using FluentAssertions;
using Microsoft.Extensions.Logging;
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
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly Mock<ILogger<CreateUserCommandHandler>> _loggerMock = new();
    private readonly ICreateUserCommandValidator _validator = new CreateUserCommandValidator();
    private readonly IUserFactory _userFactory;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _dateTimeProviderMock.Setup(p => p.GetLocalTime()).Returns(new DateTime(2025, 1, 14, 10, 0, 0));
        _userFactory = new UserFactory(_passwordHashingServiceMock.Object, _dateTimeProviderMock.Object);
        _handler = new CreateUserCommandHandler(
            _repositoryMock.Object,
            _validator,
            _userFactory,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenUsernameIsMissing()
    {
        var commandRequest = new CreateUserCommandRequest(string.Empty, "John", "Doe", "john@example.com", "password123!");
        CreateUserCommand command = new CreateUserCommand(commandRequest);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.ErrorCode.Should().Be("InvalidUsername");
        _repositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenPayloadIsValid()
    {
        var commandRequest = new CreateUserCommandRequest("  DemoUser  ", " John ", " Doe ", "TEST@MAIL.com", "password123!");
        CreateUserCommand command = new CreateUserCommand(commandRequest);
        _passwordHashingServiceMock.Setup(p => p.Hash(command.Request.Password)).Returns("hashed-value");

        User? capturedUser = null;
        DateTime now = new DateTime(2025, 1, 14, 12, 0, 0);
        _dateTimeProviderMock.Setup(p => p.GetLocalTime()).Returns(now);
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
        capturedUser.CreatedAt.Should().Be(now);
        capturedUser.UpdateAt.Should().Be(now);
        capturedUser.Username.Should().Be("DemoUser");
        capturedUser.Email.Should().Be("test@mail.com");
    }
}
