using Microsoft.Extensions.Options;
using SocialMedia.Users.Domain.Services;
using SocialMedia.Users.Infrastructure.Configuration;

namespace SocialMedia.Users.Infrastructure.Services;

public class TimeZoneProvider(IOptions<TimeZoneSettings> options) : IDateTimeProvider
{
    private readonly TimeZoneSettings _settings = options.Value;

    public DateTime GetLocalTime()
    {
        foreach (string timeZoneId in _settings.PreferredTimeZoneIds)
        {
            if (string.IsNullOrWhiteSpace(timeZoneId))
            {
                continue;
            }

            try
            {
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                continue;
            }
            catch (InvalidTimeZoneException)
            {
                continue;
            }
        }

        return DateTime.Now;
    }
}
