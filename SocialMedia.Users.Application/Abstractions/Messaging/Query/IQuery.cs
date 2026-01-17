using MediatR;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Abstractions.Messaging.Query;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
