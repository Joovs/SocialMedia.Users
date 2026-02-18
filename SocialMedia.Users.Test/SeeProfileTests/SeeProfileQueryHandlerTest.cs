using FluentAssertions;
using Moq;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Application.Users.Queries.SeeProfile;
using SocialMedia.Users.Domain.Entities;

namespace SocialMedia.Users.Test.SeeProfileTests;
public class SeeProfileQueryHandlerTest
{
    private readonly Mock<IUserRepository> _mockIUserRepository = new();
    private readonly CancellationToken _cancellationToken = new();

    [Fact]
    public async Task seeProfileQueryHandler_Success()
    {
        var userId = Guid.NewGuid();
        UserProfile userProfile = new();
        SeeProfileQueryResponse seeProfileQueryResponse = new();
        _mockIUserRepository.Setup(x => x.SeeProfile(userId, _cancellationToken)).ReturnsAsync(userProfile);

        var query = new SeeProfileQuery(userId);
        var queryHandler = new SeeProfileQueryHandler(_mockIUserRepository.Object);
        var result = await queryHandler.Handle(query, _cancellationToken);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task seeProfileQueryHandler_Bad_Request()
    {
        UserProfile userProfile = new();
        SeeProfileQueryResponse seeProfileQueryResponse = new();
        _mockIUserRepository.Setup(x => x.SeeProfile(Guid.Empty, _cancellationToken)).ReturnsAsync(userProfile);

        var query = new SeeProfileQuery(Guid.Empty);
        var queryHandler = new SeeProfileQueryHandler(_mockIUserRepository.Object);
        var result = await queryHandler.Handle(query, _cancellationToken);

        // Assert
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("BadRequest", result.Error?.ErrorCode);
        Assert.True(!result.IsSuccess);
    }
}