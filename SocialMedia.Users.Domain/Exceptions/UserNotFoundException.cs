namespace SocialMedia.Users.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException()
        : base("The user was not found.") { }
}
