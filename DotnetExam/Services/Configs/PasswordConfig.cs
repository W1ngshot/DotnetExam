namespace DotnetExam.Services.Configs;

public static class PasswordConfig
{
    public const int MinimumLength = 1;
    
    public const bool RequireNonAlphanumeric = false;
    
    public const bool RequireLowercase = false;
    
    public const bool RequireUppercase = false;
    
    public const bool RequireDigit = false;
}