using Moq;
using SocialMedia.Users.Application.Abstractions;
using SocialMedia.Users.Application.Commands.Follow;
using SocialMedia.Users.Domain.Entities;

namespace SocialMedia.Users.Test.Commands.Follow;

public class UnfollowUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<SocialMedia.Users.Domain.Entities.Follow>> _mockFollowRepository;
    private readonly UnfollowUserCommandHandler _handler;

    public UnfollowUserCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockFollowRepository = new Mock<IRepository<SocialMedia.Users.Domain.Entities.Follow>>();
        
        _mockUnitOfWork.Setup(x => x.Follows).Returns(_mockFollowRepository.Object);
        
        _handler = new UnfollowUserCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithActiveFollow_ShouldDeactivateFollow()
    {
        // Arrange
        var followerId = Guid.NewGuid();
        var followingId = Guid.NewGuid();
        var command = new UnfollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        var activeFollow = new SocialMedia.Users.Domain.Entities.Follow
        {
            FollowerUserId = followerId,
            FollowingUserId = followingId,
            IsActive = true
        };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync(activeFollow);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(followerId, result.FollowerUserId);
        Assert.Equal(followingId, result.FollowingUserId);
        Assert.Equal("Unfollowed", result.Status);
        
        _mockFollowRepository.Verify(x => x.Update(It.IsAny<SocialMedia.Users.Domain.Entities.Follow>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentFollow_ShouldThrowException()
    {
        // Arrange
        var followerId = Guid.NewGuid();
        var followingId = Guid.NewGuid();
        var command = new UnfollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync((SocialMedia.Users.Domain.Entities.Follow?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command));
        Assert.Equal("No sigues a este usuario", exception.Message);
    }

    [Fact]
    public async Task Handle_WithInactiveFollow_ShouldThrowException()
    {
        // Arrange
        var followerId = Guid.NewGuid();
        var followingId = Guid.NewGuid();
        var command = new UnfollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        var inactiveFollow = new SocialMedia.Users.Domain.Entities.Follow
        {
            FollowerUserId = followerId,
            FollowingUserId = followingId,
            IsActive = false
        };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync(inactiveFollow);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command));
        Assert.Equal("No sigues a este usuario", exception.Message);
    }
}
