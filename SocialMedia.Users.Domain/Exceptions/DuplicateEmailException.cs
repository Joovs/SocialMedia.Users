namespace SocialMedia.Users.Domain.Exceptions;

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException()
            : base("The email address is already registered.") { }

    public DuplicateEmailException(string email)
        : base($"The email '{email}' is already registered.") { }
}
