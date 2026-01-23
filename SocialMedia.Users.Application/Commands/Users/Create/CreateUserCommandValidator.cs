using System.ComponentModel.DataAnnotations;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Commands.Users.Create;

public class CreateUserCommandValidator : ICreateUserCommandValidator
{
    public Result<CreateUserCommandResponse>? Validate(CreateUserCommand command)
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
}
