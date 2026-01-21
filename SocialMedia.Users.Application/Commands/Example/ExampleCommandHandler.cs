using MediatR;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities.UserEntity;

namespace SocialMedia.Users.Application.Commands.Example;

public class ExampleCommandHandler(IUserRepository repository) : IRequestHandler<ExampleCommand, Result<ExampleCommandResponse>>
{
    private readonly IUserRepository _repository = repository;
    public async Task<Result<ExampleCommandResponse>> Handle(ExampleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            User updateUserExample = await _repository.ExampleUpdateUser(request.userId, cancellationToken);

            ExampleCommandResponse response = new ExampleCommandResponse
            {
                UserId = updateUserExample.Id,
                UserName = updateUserExample.Username
            };

            return Result<ExampleCommandResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<ExampleCommandResponse>.Failure(500, "InternalErrorServer", ex.Message);
        }
    }
}
