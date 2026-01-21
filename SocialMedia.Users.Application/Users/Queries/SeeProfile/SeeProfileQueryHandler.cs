using MediatR;
using SocialMedia.Users.Application.DTO;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities;

namespace SocialMedia.Users.Application.Users.Queries.SeeProfile;

public class SeeProfileQueryHandler(IUserRepository repository) : IRequestHandler<SeeProfileQuery, Result<SeeProfileQueryResponse>>
{
    private readonly IUserRepository _repository = repository;
    public async Task<Result<SeeProfileQueryResponse>> Handle(SeeProfileQuery request, CancellationToken cancellationToken)
    {
        if (request is null || request.userId == Guid.Empty)
        {
            return Result<SeeProfileQueryResponse>.Failure(400, "BadRequest", "Invalid Data");
        }
        try
        {
            UserProfile userProfile = await _repository.SeeProfile(request.userId, cancellationToken);

            if (userProfile is null)
            {
                return Result<SeeProfileQueryResponse>.Failure(401, "Unauthorized", "Invalid Credentials");
            }

            return Result<SeeProfileQueryResponse>.Success(InstantiateSeeProfileResponse(userProfile));
        }
        catch (TaskCanceledException)
        {
            return Result<SeeProfileQueryResponse>.Failure(500, "TaskCanceledException", "The operation was canceled before completion.");
        }
    }
    
     private static SeeProfileQueryResponse InstantiateSeeProfileResponse(UserProfile userP)
    {
        return new()
        {
            Id = userP.Id,
            Username = userP.Username,
            Lastname = userP.Lastname,
            CreatedAt = userP.CreatedAt,
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