using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Application.Abstractions;
using SocialMedia.Users.Infrastructure.Persistence.Context;
using SocialMedia.Users.Infrastructure.Persistence.Repositories;
using SocialMedia.Users.Domain.Entities.UserEntity;

namespace SocialMedia.Users.Test.Infrastructure.Repositories;

public class RepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public RepositoryTests()
    {
        // Usar base de datos en memoria para pruebas
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task AddAsync_WithValidEntity_ShouldAddEntity()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new Repository<User>(context);
        
        var user = new User { Username = "testuser", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

        // Act
        await repository.AddAsync(user);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await repository.FindAsync(user.Id);
        Assert.NotNull(savedUser);
        Assert.Equal("testuser", savedUser.Username);
    }

    [Fact]
    public async Task FindAsync_WithValidId_ShouldReturnEntity()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new Repository<User>(context);
        
        var user = new User { Username = "testuser", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var userId = user.Id;

        // Act
        var foundUser = await repository.FindAsync(userId);

        // Assert
        Assert.NotNull(foundUser);
        Assert.Equal(userId, foundUser.Id);
    }

    [Fact]
    public async Task FindAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new Repository<User>(context);

        // Act
        var foundUser = await repository.FindAsync(9999);

        // Assert
        Assert.Null(foundUser);
    }

    [Fact]
    public async Task Update_WithValidEntity_ShouldUpdateEntity()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new Repository<User>(context);
        
        var user = new User { Username = "oldname", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var userId = user.Id;

        // Act
        user.Username = "newname";
        repository.Update(user);
        await context.SaveChangesAsync();

        // Assert
        var updatedUser = await repository.FindAsync(userId);
        Assert.NotNull(updatedUser);
        Assert.Equal("newname", updatedUser.Username);
    }

    [Fact]
    public async Task Remove_WithValidEntity_ShouldRemoveEntity()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new Repository<User>(context);
        
        var user = new User { Username = "testuser", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var userId = user.Id;

        // Act
        repository.Remove(user);
        await context.SaveChangesAsync();

        // Assert
        var deletedUser = await repository.FindAsync(userId);
        Assert.Null(deletedUser);
    }

    [Fact]
    public void AsQueryable_ShouldReturnQueryable()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new Repository<User>(context);
        
        context.Users.Add(new User { Username = "user1", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
        context.Users.Add(new User { Username = "user2", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
        context.SaveChanges();

        // Act
        var query = repository.AsQueryable();
        var users = query.ToList();

        // Assert
        Assert.Equal(2, users.Count);
    }
}
