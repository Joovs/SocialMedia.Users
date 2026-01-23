using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Application.Abstractions;
using SocialMedia.Users.Infrastructure.Persistence;
using SocialMedia.Users.Infrastructure.Persistence.Context;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities;

namespace SocialMedia.Users.Test.Infrastructure;

public class UnitOfWorkTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public UnitOfWorkTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void Users_ShouldReturnRepositoryInstance()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var unitOfWork = new UnitOfWork(context);

        // Act
        var usersRepository = unitOfWork.Users;

        // Assert
        Assert.NotNull(usersRepository);
        Assert.IsAssignableFrom<IRepository<User>>(usersRepository);
    }

    [Fact]
    public void Follows_ShouldReturnRepositoryInstance()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var unitOfWork = new UnitOfWork(context);

        // Act
        var followsRepository = unitOfWork.Follows;

        // Assert
        Assert.NotNull(followsRepository);
        Assert.IsAssignableFrom<IRepository<Follow>>(followsRepository);
    }

    [Fact]
    public void Users_ShouldReturnSameInstanceOnMultipleCalls()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var unitOfWork = new UnitOfWork(context);

        // Act
        var usersRepository1 = unitOfWork.Users;
        var usersRepository2 = unitOfWork.Users;

        // Assert
        Assert.Same(usersRepository1, usersRepository2);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistChanges()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var unitOfWork = new UnitOfWork(context);
        
        var user = new User { Username = "testuser", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        await unitOfWork.Users.AddAsync(user);

        // Act
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        Assert.True(result > 0);
        
        using var verifyContext = new ApplicationDbContext(_dbContextOptions);
        var savedUser = await verifyContext.Users.FindAsync(user.Id);
        Assert.NotNull(savedUser);
    }

    [Fact]
    public async Task SaveChangesAsync_WithFollowOperation_ShouldPersist()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var unitOfWork = new UnitOfWork(context);
        
        var followerId = Guid.NewGuid();
        var followingId = Guid.NewGuid();
        
        var follow = new Follow
        {
            FollowerUserId = followerId,
            FollowingUserId = followingId,
            IsActive = true
        };
        
        await unitOfWork.Follows.AddAsync(follow);

        // Act
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        Assert.True(result > 0);
        
        using var verifyContext = new ApplicationDbContext(_dbContextOptions);
        var savedFollow = await verifyContext.Follows.FindAsync(followerId, followingId);
        Assert.NotNull(savedFollow);
        Assert.True(savedFollow.IsActive);
    }
}
