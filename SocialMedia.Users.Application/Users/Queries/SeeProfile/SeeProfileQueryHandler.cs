using MediatR;
using SocialMedia.Users.Application.DTO;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;

namespace SocialMedia.Users.Application.Users.Queries.SeeProfile;

public class SeeProfileQueryHandler(IUserRepository repository) : IRequestHandler<SeeProfileQuery, Result<SeeProfileQueryResponse>>
{
    private readonly IUserRepository _repository = repository;
    private readonly CancellationToken cancellationToken;
    public async Task<Result<SeeProfileQueryResponse>> Handle(SeeProfileQuery request, CancellationToken cancellationToken)
    {
        if (request is null || request.userId == Guid.Empty)
        {
            return Result<SeeProfileQueryResponse>.Failure(400, "Bad Request", "Invalid Credentials");
        }
        try
        {
            UserProfile userProfile = await _repository.SeeProfile(request.userId, cancellationToken);

            if (userProfile is null)
            {
                return Result<SeeProfileQueryResponse>.Failure(404, "UserNotFoudException", "User not found");
            }

            return Result<SeeProfileQueryResponse>.Success(InstantiateSeeProfileResponse(userProfile));
        }
        catch (Exception ex)
        {
            return Result<SeeProfileQueryResponse>.Failure(500, "InternalErrorServer", ex.Message);
        }
    }
    
     private static SeeProfileQueryResponse InstantiateSeeProfileResponse(UserProfile userP)
    {
        return new()
        {
            Id = userP.Id,
            Username = userP.Username,
            Lastname = userP.Lastname,
            Password = userP.Password,
            CreatedAt = userP.CreatedAt,
            UpdateAt = userP.UpdateAt,
            Posted = userP.Posted?.Select(post => new PostDTO
            {
                Id = post.Id,
                UserId = post.UserId,
                Body = post.Body,
                CreatedAt = post.CreatedAt,
            }).ToList(),
        };
    }
}