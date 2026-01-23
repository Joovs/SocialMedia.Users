using Moq;
using SocialMedia.Users.Application.Abstractions;
using SocialMedia.Users.Application.Commands.Follow.UnfollowUserCommand;
using SocialMedia.Users.Domain.Entities;

namespace SocialMedia.Users.Test.Commands.Follow;

public class UnfollowUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Follow>> _mockFollowRepository;
    private readonly UnfollowUserCommandHandler _handler;

    public UnfollowUserCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockFollowRepository = new Mock<IRepository<Follow>>();
        
        _mockUnitOfWork.Setup(x => x.Follows).Returns(_mockFollowRepository.Object);
        
        _handler = new UnfollowUserCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithActiveFollow_ShouldDeactivateFollow()
    {
        // Arrange
        Guid followerId = Guid.NewGuid();
        Guid followingId = Guid.NewGuid();
        UnfollowUserCommand command = new UnfollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        Follow activeFollow = new Follow
        {
            FollowerUserId = followerId,
            FollowingUserId = followingId,
            IsActive = true
        };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync(activeFollow);

        // Act
        UnfollowCommandResponse result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(followerId, result.FollowerUserId);
        Assert.Equal(followingId, result.FollowingUserId);
        Assert.Equal("Unfollowed", result.Status);
        
        _mockFollowRepository.Verify(x => x.Update(It.IsAny<Follow>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentFollow_ShouldThrowException()
    {
        // Arrange
        Guid followerId = Guid.NewGuid();
        Guid followingId = Guid.NewGuid();
        UnfollowUserCommand command = new UnfollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync((Follow?)null);

        // Act & Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("You are not following this user", exception.Message);
    }

    [Fact]
    public async Task Handle_WithInactiveFollow_ShouldThrowException()
    {
        // Arrange
        Guid followerId = Guid.NewGuid();
        Guid followingId = Guid.NewGuid();
        UnfollowUserCommand command = new UnfollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        Follow inactiveFollow = new Follow
        {
            FollowerUserId = followerId,
            FollowingUserId = followingId,
            IsActive = false
        };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync(inactiveFollow);

        // Act & Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("You are not following this user", exception.Message);
    }
}

