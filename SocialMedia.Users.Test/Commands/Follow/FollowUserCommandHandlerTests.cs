using Moq;
using SocialMedia.Users.Application.Abstractions;
using SocialMedia.Users.Application.Commands.Follow;
using SocialMedia.Users.Domain.Entities;

namespace SocialMedia.Users.Test.Commands.Follow;

public class FollowUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<SocialMedia.Users.Domain.Entities.Follow>> _mockFollowRepository;
    private readonly FollowUserCommandHandler _handler;

    public FollowUserCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockFollowRepository = new Mock<IRepository<SocialMedia.Users.Domain.Entities.Follow>>();
        
        _mockUnitOfWork.Setup(x => x.Follows).Returns(_mockFollowRepository.Object);
        
        _handler = new FollowUserCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidFollowRequest_ShouldCreateFollow()
    {
        // Arrange
        var followerId = Guid.NewGuid();
        var followingId = Guid.NewGuid();
        var command = new FollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync((SocialMedia.Users.Domain.Entities.Follow?)null);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(followerId, result.FollowerUserId);
        Assert.Equal(followingId, result.FollowingUserId);
        Assert.Equal("Followed", result.Status);
        
        _mockFollowRepository.Verify(x => x.AddAsync(It.IsAny<SocialMedia.Users.Domain.Entities.Follow>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithSelfFollow_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new FollowUserCommand { FollowerUserId = userId, FollowingUserId = userId };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command));
        Assert.Equal("No puedes seguirte a ti mismo", exception.Message);
    }

    [Fact]
    public async Task Handle_WithAlreadyFollowing_ShouldThrowException()
    {
        // Arrange
        var followerId = Guid.NewGuid();
        var followingId = Guid.NewGuid();
        var command = new FollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        var existingFollow = new SocialMedia.Users.Domain.Entities.Follow
        {
            FollowerUserId = followerId,
            FollowingUserId = followingId,
            IsActive = true
        };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync(existingFollow);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command));
        Assert.Equal("Ya sigues a este usuario", exception.Message);
    }

    [Fact]
    public async Task Handle_WithInactiveFollow_ShouldActivateFollow()
    {
        // Arrange
        var followerId = Guid.NewGuid();
        var followingId = Guid.NewGuid();
        var command = new FollowUserCommand { FollowerUserId = followerId, FollowingUserId = followingId };
        
        var inactiveFollow = new SocialMedia.Users.Domain.Entities.Follow
        {
            FollowerUserId = followerId,
            FollowingUserId = followingId,
            IsActive = false
        };
        
        _mockFollowRepository.Setup(x => x.FindAsync(followerId, followingId))
            .ReturnsAsync(inactiveFollow);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Followed", result.Status);
        
        _mockFollowRepository.Verify(x => x.Update(It.IsAny<SocialMedia.Users.Domain.Entities.Follow>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
