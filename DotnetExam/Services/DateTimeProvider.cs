using DotnetExam.Services.Interfaces;

namespace DotnetExam.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}