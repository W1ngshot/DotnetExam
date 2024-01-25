namespace DotnetExam.Services.Configs;

public static class TokensConfig
{
    public const int TokenLifetime = 10 * 60;

    public const int RefreshTokenLifetime = 7 * 24 * 60 * 60;
}
