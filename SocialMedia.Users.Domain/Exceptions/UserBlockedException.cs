namespace SocialMedia.Users.Domain.Exceptions;

public sealed class UserBlockedException : Exception
{
    public UserBlockedException(string message) : base(message) {}
}