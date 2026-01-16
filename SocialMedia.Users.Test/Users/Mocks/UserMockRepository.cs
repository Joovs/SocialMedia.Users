using SocialMedia.Users.Application.Common.Validations;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Models.UpdateProfile;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;
using SocialMedia.Users.Domain.Exceptions;
using System.Linq;

namespace SocialMedia.Users.Test.Users.Mocks;

public class UserMockRepository : IUserRepository
{
    private readonly List<User> _database = new()
    {
        new User
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b11"),
            Username = "UserTest",
            FirstName = "UserTestFirstname",
            LastName = "UserTestLastname",
            Email = "user@test.com",
            Password = "UserTest1!",
            CreatedAt = new DateTime(2026, 9, 1),
            UpdateAt = new DateTime(2026, 1, 25)
        },
        new User
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b12"),
            Username = "UserTest2",
            FirstName = "UserTestFirstname2",
            LastName = "UserTestLastname2",
            Email = "user2@test.com",
            Password = "UserTest2!",
            CreatedAt = new DateTime(2026, 9, 1),
            UpdateAt = new DateTime(2026, 1, 25)
        },
        new User
        {
            Id = new Guid("d2719c3a-5f1b-4f67-9c4e-5a2f7c9c9b13"),
            Username = "UserTest3",
            FirstName = "UserTestFirstname3",
            LastName = "UserTestLastname3",
            Email = "user3@test.com",
            Password = "UserTest3!",
            CreatedAt = new DateTime(2026, 9, 1),
            UpdateAt = new DateTime(2026, 1, 25)
        }
    };
    public Task<UpdateProfileResponseModel> UpdateProfile(UpdateProfileModel request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException("Request can not be null");
        }

        if (!ValidationRules.IsValidId(request.Id))
        {
            throw new ArgumentException("Invalid ID");
        }

        if (!ValidationRules.HasCorrectLength(request.Username, 255))
        {
            throw new ArgumentException("Invalid username");
        }

        if (!ValidationRules.HasCorrectLength(request.FirstName, 255))
        {
            throw new ArgumentException("Invalid firstname");
        }

        if (!ValidationRules.HasCorrectLength(request.LastName, 255))
        {
            throw new ArgumentException("Invalid lastname");
        }

        if (!ValidationRules.HasCorrectLength(request.Email, 255) ||
            !ValidationRules.IsValidEmail(request.Email))
        {
            throw new ArgumentException("Invalid email");
        }

        if (!ValidationRules.HasCorrectLength(request.Password, 255) ||
            !ValidationRules.IsValidPassword(request.Password))
        {
            throw new ArgumentException("Invalid password");
        }
        User? user = _database.FirstOrDefault(u => u.Id == request.Id);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found");
        }

        bool exists = _database.Any(u =>
            u.Email == request.Email && u.Id != request.Id);

        if (exists)
        {
            throw new DuplicateEmailException(request.Email);
        }


        // update simulation
        user.Username = request.Username;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.Password = request.Password;
        user.UpdateAt = DateTime.Now;

        UpdateProfileResponseModel response = new UpdateProfileResponseModel
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            UpdatedAt = user.UpdateAt
        };

        return Task.FromResult(response);
        
    }
}
