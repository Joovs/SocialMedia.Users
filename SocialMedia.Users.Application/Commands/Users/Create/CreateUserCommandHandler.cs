using MediatR;
using Microsoft.Extensions.Logging;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;

namespace SocialMedia.Users.Application.Commands.Users.Create;

public class CreateUserCommandHandler(
    IUserRepository repository,
    ICreateUserCommandValidator validator,
    IUserFactory userFactory,
    ILogger<CreateUserCommandHandler> logger) : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
{
    private readonly IUserRepository _repository = repository;
    private readonly ICreateUserCommandValidator _validator = validator;
    private readonly IUserFactory _userFactory = userFactory;
    private readonly ILogger<CreateUserCommandHandler> _logger = logger;

    public async Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Result<CreateUserCommandResponse>? validationResult = _validator.Validate(request);
            if (validationResult is not null)
            {
                return validationResult;
            }

            User newUser = _userFactory.CreateNew(request);

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
            _logger.LogError(ex, "Error creating user {Username}", request.Username);
            return Result<CreateUserCommandResponse>.Failure(500, "CreateUserError", ex.Message);
        }
    }

}
