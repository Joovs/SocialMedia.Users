using System.ComponentModel.DataAnnotations;
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
            string normalizedLastname = request.Lastname.Trim();
            string normalizedEmail = request.Email.Trim().ToLowerInvariant();
            string hashedPassword = _passwordHashingService.Hash(request.Password);

            User newUser = new User
            {
                Username = normalizedUsername,
                Lastname = normalizedLastname,
                Email = normalizedEmail,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            User createdUser = await _repository.CreateUserAsync(newUser, cancellationToken);

            var response = new CreateUserCommandResponse
            {
                UserId = createdUser.Id,
                Username = createdUser.Username,
                Lastname = createdUser.Lastname,
                Email = createdUser.Email,
                CreatedAt = createdUser.CreatedAt
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

        if (string.IsNullOrWhiteSpace(command.Lastname))
        {
            return Result<CreateUserCommandResponse>.Failure(400, "InvalidLastname", "Lastname is required.");
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
}
