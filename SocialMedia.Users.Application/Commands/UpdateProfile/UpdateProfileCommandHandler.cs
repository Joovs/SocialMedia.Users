using MediatR;
using SocialMedia.Users.Application.Common.Validations;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities.UserEntity.Models.UpdateProfile;
using SocialMedia.Users.Domain.Exceptions;
using SocialMedia.Users.Domain.Services.Hasher;
using System.Text.RegularExpressions;

namespace SocialMedia.Users.Application.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<UpdateProfileCommandResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;

    public UpdateProfileCommandHandler(IUserRepository userRepository, IHasher hasher)
    {
        _userRepository = userRepository;
        _hasher = hasher;
    }

    public async Task<Result<UpdateProfileCommandResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {

            if (request is null)
            {
                return Result<UpdateProfileCommandResponse>.Failure(400, "BadRequest", "Request cannot be null");
            }
            
            if (!ValidationRules.IsValidId(request.request.Id))
            {
                return Result<UpdateProfileCommandResponse>.Failure(400, "Invalid ID", "ID must be a valid Guid");
            }

            if (!ValidationRules.HasCorrectLength(request.request.Username, 255))
            {
                return Result<UpdateProfileCommandResponse>.Failure(400, "Invalid username", "Username must be greater than 0 and less than 255 characters");
            }

            if (!ValidationRules.HasCorrectLength(request.request.FirstName, 255))
            {
                return Result<UpdateProfileCommandResponse>.Failure(400, "BadRequest", "Firstname must be greater than 0 and less than 255 characters");
            }

            if (!ValidationRules.HasCorrectLength(request.request.LastName, 255))
            {
                return Result<UpdateProfileCommandResponse>.Failure(400, "BadRequest", "Lastname must be greater than 0 and less than 255 characters");
            }

            if (!ValidationRules.HasCorrectLength(request.request.Email, 255) ||
                !ValidationRules.IsValidEmail(request.request.Email))
            {
                return Result<UpdateProfileCommandResponse>.Failure(400, "Invalid email", "You must add a valid email address");
            }

            if (!ValidationRules.HasCorrectLength(request.request.Password, 255) ||
                !ValidationRules.IsValidPassword(request.request.Password))
            {
                return Result<UpdateProfileCommandResponse>.Failure(400, "Invalid password", "The password must be at least 8 characters long, and include one lowercase letter, one uppercase letter, one number, and one special character.");
            }

            bool userExists = await _userRepository.UserExists(request.request.Id, cancellationToken);

            if (!userExists)
            {
                return Result<UpdateProfileCommandResponse>.Failure(400, "ArgumentExeption", "InvalidID");
            }

            string passwordHashed = _hasher.hashPassword(request.request.Password);

            UpdateProfileModel model = new UpdateProfileModel
            {
                Id = request.request.Id,
                Username = request.request.Username,
                FirstName = request.request.FirstName,
                LastName = request.request.LastName,
                Email = request.request.Email,
                Password = passwordHashed
            };

            UpdateProfileResponseModel response = await _userRepository.UpdateProfile(model, cancellationToken);


            UpdateProfileCommandResponse profileUpdated = new UpdateProfileCommandResponse
            {
                Message = "User profile successfully updated",
                UpdatedAt = response.UpdatedAt
            };

            return Result<UpdateProfileCommandResponse>.Success(profileUpdated);
        }
        catch (DuplicateEmailException ex)
        {
            return Result<UpdateProfileCommandResponse>.Failure(409, "InvalidEmail", ex.Message);
        }
        catch (Exception ex)
        {
            return Result<UpdateProfileCommandResponse>.Failure(
                500, "InternalServerError", ex.Message);
        }
    }
}
