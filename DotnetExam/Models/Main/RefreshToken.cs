namespace DotnetExam.Models.Main;

public record RefreshToken(string Token, DateTime CreatedAt, DateTime Expires)
{
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => !IsExpired;
}