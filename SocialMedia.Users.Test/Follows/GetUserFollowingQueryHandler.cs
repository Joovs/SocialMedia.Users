using FluentAssertions;
using Moq;
using SocialMedia.Users.Application.Queries.GetUserFollowing;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Domain.Entities.FollowEntity;

public class GetUserFollowingQueryHandlerTest
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IFollowRepository> _followRepository;
    private readonly GetUserFollowingQueryHandler _handler;

    public GetUserFollowingQueryHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _followRepository = new Mock<IFollowRepository>();

        _handler = new GetUserFollowingQueryHandler(
            _userRepository.Object,
            _followRepository.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Fail_When_User_Does_Not_Exist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepository
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var query = new GetUserFollowingQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.ErrorCode.Should().Be("USER_NOT_FOUND");
    }

    [Fact]
    public async Task Handle_Should_Fail_When_UserId_Is_Empty()
    {
        // Arrange
        var query = new GetUserFollowingQuery(Guid.Empty);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.ErrorCode.Should().Be("INVALID_USER_ID");
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_User_Follows_No_One()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepository
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _followRepository
            .Setup(x => x.GetFollowingAsync(userId, CancellationToken.None))
            .ReturnsAsync(new List<UserFollow>());

        var query = new GetUserFollowingQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Following.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Return_Following_List_When_User_Exists()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepository
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _followRepository
            .Setup(x => x.GetFollowingAsync(userId, CancellationToken.None))
            .ReturnsAsync(new List<UserFollow>
            {
            new() { UserId = Guid.NewGuid(), Username = "alice" },
            new() { UserId = Guid.NewGuid(), Username = "bob" }
            });

        var query = new GetUserFollowingQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Following.Should().HaveCount(2);
        result.Value.Following[0].Username.Should().Be("alice");
        result.Value.Following[1].Username.Should().Be("bob");
    }





}


