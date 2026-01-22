namespace SocialMedia.Users.Domain.Exceptions;

public class InvalidRequestDataException : System.Exception
{
    public InvalidRequestDataException(string message) : base(message) { }
}