using MediatR;
using SocialMedia.Users.Application.Shared;

namespace SocialMedia.Users.Application.Abstractions.Messaging.Query;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
