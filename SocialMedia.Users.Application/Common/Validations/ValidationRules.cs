using System.Text.RegularExpressions;

namespace SocialMedia.Users.Application.Common.Validations;

public static class ValidationRules
{
    public static bool IsValidId(int id)
        => id > 0;

    public static bool HasCorrectLength(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            return false;

        return value.Length <= maxLength;
    }

    public static bool IsValidEmail(string email)
        => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public static bool IsValidPassword(string password)
        => Regex.IsMatch(password,
           @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
}
