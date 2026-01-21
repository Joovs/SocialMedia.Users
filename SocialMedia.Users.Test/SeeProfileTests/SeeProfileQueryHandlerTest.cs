using System.Threading.Tasks;
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
    public async Task SeeProfileQueryHandler_Success()
    {
        var userId = Guid.NewGuid();
        SeeProfileQueryRequest seeProfile = new()
        {
            UserId = Guid.NewGuid()
        };
        UserProfile userProfile = new();
        SeeProfileQueryResponse seeProfileQueryResponse = new();
        _mockIUserRepository.Setup(x => x.SeeProfile(userId, _cancellationToken)).ReturnsAsync(userProfile);

        var query = new SeeProfileQuery(userId);
        var queryHandler = new SeeProfileQueryHandler(_mockIUserRepository.Object);
        var result = await queryHandler.Handle(query, _cancellationToken);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}