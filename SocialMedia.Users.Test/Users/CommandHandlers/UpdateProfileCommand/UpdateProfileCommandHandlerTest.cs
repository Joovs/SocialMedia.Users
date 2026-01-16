using SocialMedia.Users.Domain.Entities.UserEntity.Models.UpdateProfile;
using SocialMedia.Users.Domain.Exceptions;
using SocialMedia.Users.Test.Users.Mocks;
using Xunit;

namespace SocialMedia.Users.Test.Users.CommandHandlers.UpdateProfileCommand;

public class UpdateProfileCommandHandlerTest
{
    private readonly UserMockRepository _repository = new UserMockRepository();
    private readonly CancellationToken cancellationToken = new CancellationToken();

    


    //Happy path 
    [Fact]
    public async Task handleShouldUpdateAndReturnAUserProfile()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b11"),
            Username = "UserTest",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "user@test.com",
            Password = "UserTest1!",
        };

        //Act
        UpdateProfileResponseModel response = await _repository.UpdateProfile(model, cancellationToken);


        //Assert
        Assert.NotNull(response);
        Assert.Equal(model.Id, response.Id);
        Assert.True(response.UpdatedAt <=  DateTime.Now);
    }

    
    [Fact]
    public async Task handleShouldReturnKeyNotFoundException()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b18"),
            Username = "UserTest",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "user@test.com",
            Password = "UserTest1!",
        };

        //Act
        Func<Task> act = () => 
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(act);
    }

    
    [Fact]
    public async Task handleShouldReturnArgumentExceptionInvalidId()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("00000000-0000-0000-0000-000000000000"),
            Username = "UserTest",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "user@test.com",
            Password = "UserTest1!",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task handleShouldReturnArgumentExceptionEmptyUsername()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b11"),
            Username = "",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "user@test.com",
            Password = "UserTest1!",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task handleShouldReturnArgumentExceptionInvalidUsername()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b11"),
            Username = "jsadfkjfdsklsadlkfjksdlfjlksdjflksdjflkasdjflkasjdflkjasdñlfkjasdlñkfjasñdlkfjasñldkfjsalñdkfjasldkjflañskdjfalñskdjflsañkdjflñasdkjflaksjdflñkasdjflñkjasldñkfjasdñlfkjasldkfjlasjflñasjkdflsajflñasjfdñjasñldkfjañskdjfñlaskdjfñlaksjdfñlkasjdfñlkasjdflkjñals",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "user@test.com",
            Password = "UserTest1!",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task handleShouldReturnArgumentExceptionEmptyLastname()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b11"),
            Username = "username",
            FirstName = "UserTestFirstname",
            LastName = "",
            Email = "user@test.com",
            Password = "UserTest1!",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task handleShouldReturnArgumentExceptionInvalidLastname()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b11"),
            Username = "Usename",
            FirstName = "UserTestFirstname",
            LastName = "jsadfkjfdsklsadlkfjksdlfjlksdjflksdjflkasdjflkasjdflkjasdñlfkjasdlñkfjasñdlkfjasñldkfjsalñdkfjasldkjflañskdjfalñskdjflsañkdjflñasdkjflaksjdflñkasdjflñkjasldñkfjasdñlfkjasldkfjlasjflñasjkdflsajflñasjfdñjasñldkfjañskdjfñlaskdjfñlaksjdfñlkasjdfñlkasjdflkjñals",
            Email = "user@test.com",
            Password = "UserTest1!",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task handleShouldReturnArgumentExceptionEmptyEmail()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b11"),
            Username = "username",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "",
            Password = "UserTest1!",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task handleShouldReturnArgumentExceptionInvalidEmail()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b11"),
            Username = "Usename",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "jsadfkjfdsklsadlkfjksdlfjlksdjflksdjflkasdjflkasjdflkjasdñlfkjasdlñkfjasñdlkfjasñldkfjsalñdkfjasldkjflañskdjfalñskdjflsañkdjflñasdkjflaksjdflñkasdjflñkjasldñkfjasdñlfkjasldkfjlasjflñasjkdflsajflñasjfdñjasñldkfjañskdjfñlaskdjfñlaksjdfñlkasjdfñlkasjdflkjñals@test.com",
            Password = "UserTest1!",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task handleShouldReturnArgumentExceptionInvalidFormatEmail()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b11"),
            Username = "Usename",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "email",
            Password = "UserTest1!",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task handleShouldReturnArgumentExceptionEmptyPassword()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b12"),
            Username = "username",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "email@email.com",
            Password = "",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task handleShouldReturnArgumentExceptionInvalidPassword()
    {
        //Arrange 
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b12"),
            Username = "Usename",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "email@test.com",
            Password = "UserTestPassword",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task handleShouldReturnDuplicateEmailException()
    {
        UpdateProfileModel model = new UpdateProfileModel
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b11"),
            Username = "Usename",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLasname",
            Email = "user2@test.com",
            Password = "UserTest1!",
        };

        //Act
        Func<Task> act = () =>
        _repository.UpdateProfile(model, cancellationToken);


        //Assert
        await Assert.ThrowsAsync<DuplicateEmailException>(act);

    }
        
}
