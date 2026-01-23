using Moq;
using SocialMedia.Users.Application.Abstractions;
using SocialMedia.Users.Application.Commands.Follow.FollowUserCommand;
using SocialMedia.Users.Domain.Entities;

namespace SocialMedia.Users.Test.Commands.Follow;

public class FollowUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Follow>> _mockFollowRepository;
    private readonly FollowUserCommandHandler _handler;

    public FollowUserCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockFollowRepository = new Mock<IRepository<Follow>>();
        
        _mockUnitOfWork.Setup(x => x.Follows).Returns(_mockFollowRepository.Object);
        
        _handler = new FollowUserCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidFollowRequest_ShouldCreateFollow()
    {
        // Arrange
        Guid followerId = Guid.NewGuid();
        Guid followingId = Guid.NewGuid();
        FollowUserCommand command = new FollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync((Follow?)null);

        // Act
        FollowCommandResponse result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(followerId, result.FollowerUserId);
        Assert.Equal(followingId, result.FollowingUserId);
        Assert.Equal("Followed", result.Status);
        
        _mockFollowRepository.Verify(x => x.AddAsync(It.IsAny<Follow>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithSelfFollow_ShouldThrowException()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        FollowUserCommand command = new FollowUserCommand { FollowerUserId = userId, FollowingUserId = userId };

        // Act & Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("You cannot follow yourself", exception.Message);
    }

    [Fact]
    public async Task Handle_WithAlreadyFollowing_ShouldThrowException()
    {
        // Arrange
        Guid followerId = Guid.NewGuid();
        Guid followingId = Guid.NewGuid();
        FollowUserCommand command = new FollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        Follow existingFollow = new Follow
        {
            FollowerUserId = followerId,
            FollowingUserId = followingId,
            IsActive = true
        };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync(existingFollow);

        // Act & Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("You are already following this user", exception.Message);
    }

    [Fact]
    public async Task Handle_WithInactiveFollow_ShouldActivateFollow()
    {
        // Arrange
        Guid followerId = Guid.NewGuid();
        Guid followingId = Guid.NewGuid();
        FollowUserCommand command = new FollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        Follow inactiveFollow = new Follow
        {
            FollowerUserId = followerId,
            FollowingUserId = followingId,
            IsActive = false
        };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync(inactiveFollow);

        // Act
        FollowCommandResponse result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Followed", result.Status);
        
        _mockFollowRepository.Verify(x => x.Update(It.IsAny<Follow>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

