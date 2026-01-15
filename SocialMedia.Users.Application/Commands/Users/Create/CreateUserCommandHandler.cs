using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using MediatR;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;
using SocialMedia.Users.Domain.Services;

namespace SocialMedia.Users.Application.Commands.Users.Create;

public class CreateUserCommandHandler(
    IUserRepository repository,
    IPasswordHashingService passwordHashingService) : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
{
    private readonly IUserRepository _repository = repository;
    private readonly IPasswordHashingService _passwordHashingService = passwordHashingService;

    public async Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Result<CreateUserCommandResponse>? validationResult = Validate(request);
            if (validationResult is not null)
            {
                return validationResult;
            }

            string normalizedUsername = request.Username.Trim();
            string normalizedFirstName = request.FirstName.Trim();
            string normalizedLastName = request.LastName.Trim();
            string normalizedEmail = request.Email.Trim().ToLowerInvariant();
            string hashedPassword = _passwordHashingService.Hash(request.Password);
            DateTime localNow = GetLocalTime();

            User newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = normalizedUsername,
                FirstName = normalizedFirstName,
                LastName = normalizedLastName,
                Email = normalizedEmail,
                Password = hashedPassword,
                CreatedAt = localNow,
                UpdateAt = localNow
            };

            User createdUser = await _repository.CreateUserAsync(newUser, cancellationToken);

            var response = new CreateUserCommandResponse
            {
                Id = createdUser.Id,
                Username = createdUser.Username,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                CreatedAt = createdUser.CreatedAt,
                UpdateAt = createdUser.UpdateAt
            };

            return Result<CreateUserCommandResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<CreateUserCommandResponse>.Failure(500, "CreateUserError", ex.Message);
        }
    }

    private static Result<CreateUserCommandResponse>? Validate(CreateUserCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Username))
        {
            return Result<CreateUserCommandResponse>.Failure(400, "InvalidUsername", "Username is required.");
        }

        if (string.IsNullOrWhiteSpace(command.FirstName))
        {
            return Result<CreateUserCommandResponse>.Failure(400, "InvalidFirstName", "FirstName is required.");
        }

        if (string.IsNullOrWhiteSpace(command.LastName))
        {
            return Result<CreateUserCommandResponse>.Failure(400, "InvalidLastName", "LastName is required.");
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            return Result<CreateUserCommandResponse>.Failure(400, "InvalidPassword", "Password is required.");
        }

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            return Result<CreateUserCommandResponse>.Failure(400, "InvalidEmail", "Email is required.");
        }

        EmailAddressAttribute emailValidator = new();
        if (!emailValidator.IsValid(command.Email))
        {
            return Result<CreateUserCommandResponse>.Failure(400, "InvalidEmailFormat", "Email format is not valid.");
        }

        return null;
    }

    private static DateTime GetLocalTime()
    {
        try
        {
            string timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "Central Standard Time (Mexico)"
                : "America/Mexico_City";

            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
        }
        catch (TimeZoneNotFoundException)
        {
            return DateTime.Now;
        }
        catch (InvalidTimeZoneException)
        {
            return DateTime.Now;
        }
    }
}
