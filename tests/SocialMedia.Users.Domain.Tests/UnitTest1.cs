using FluentAssertions;
using SocialMedia.Users.Domain.Entities.UserEntity;

namespace SocialMedia.Users.Domain.Tests;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        User user = new User();

        user.Id.Should().Be(Guid.Empty);
        user.Username.Should().BeEmpty();
        user.FirstName.Should().BeEmpty();
        user.LastName.Should().BeEmpty();
        user.Email.Should().BeEmpty();
        user.Password.Should().BeEmpty();
        user.CreatedAt.Should().Be(default);
        user.UpdateAt.Should().Be(default);
    }

    [Fact]
    public void UpdateAt_ShouldReflectLatestAssignment()
    {
        User user = new User();
        DateTime firstValue = new DateTime(2025, 1, 1);
        DateTime secondValue = new DateTime(2025, 2, 1);

        user.UpdateAt = firstValue;
        user.UpdateAt = secondValue;

        user.UpdateAt.Should().Be(secondValue);
    }
}
