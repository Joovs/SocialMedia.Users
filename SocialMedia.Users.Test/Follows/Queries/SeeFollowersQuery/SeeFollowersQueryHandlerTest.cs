using SocialMedia.Users.Domain.Entities.FollowsEntity.Models.SeeFollowers;
using SocialMedia.Users.Test.Follows.Mocks;
using System;

namespace SocialMedia.Users.Test.Follows.Queries.SeeFollowersQuery;

public class SeeFollowersQueryHandlerTest
{
    private readonly FollowMockRepository _repository = new FollowMockRepository();
    private readonly CancellationToken cancellationToken = new CancellationToken();

    //Happy path
    [Theory]
    [InlineData("6FBB9F92-3FD1-4C8A-747A-08DE53C6BA21")]
    [InlineData("6FBB9F92-3FD1-4C8A-747A-08DE53C6BA22")]
    public async Task handleShouldReturnAListOfFollowers(Guid userID)
    {
        //Arrange 
        bool userExists = await _repository.UserExists(userID, cancellationToken);
        
        if(!userExists) throw new ArgumentException(nameof(userID));

        //Act
        List<Follower> response = await _repository.SeeFollowers(userID, cancellationToken);

        //Assert
        Assert.NotNull(response);
        Assert.IsType<List<Follower>>(response);

    }
    
    [Theory]
    [InlineData("6FBB9F92-3FD1-4C8A-747A-08DE53C6BA25")]
    [InlineData("6FBB9F92-3FD1-4C8A-747A-08DE53C6BA28")]
    public async Task handleShouldReturnArgumenExcepcionWhenUserIdDoesNotExists(Guid userID)
    {
        //Arrange 

        //Act
        var result = await _repository.UserExists(userID, CancellationToken.None);

        //Assert
        Assert.False(result);
        

    }
}
