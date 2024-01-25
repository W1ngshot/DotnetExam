using Microsoft.AspNetCore.Identity;

namespace DotnetExam.Models.Main;

public class AppUser : IdentityUser<Guid>
{
    public const string RefreshTokensField = nameof(_refreshTokens);
    
    private List<RefreshToken> _refreshTokens = [];
    
    public AppUser()
    {
        Id = Guid.NewGuid();
    }

    public IReadOnlyList<RefreshToken> RefreshTokens
    {
        get => _refreshTokens;
        protected set => _refreshTokens = value as List<RefreshToken> ?? [..value];
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.Add(refreshToken);
        PurgeExpiredAndEnsureLimit();
    }
    
    public bool RemoveRefreshToken(RefreshToken refreshToken)
    {
        var removed = _refreshTokens.Remove(refreshToken);
        PurgeExpiredAndEnsureLimit();
        return removed;
    }

    private void PurgeExpiredAndEnsureLimit()
        => _refreshTokens = _refreshTokens
            .Where(x => x.IsActive)
            .OrderBy(x => x.Expires)
            .Take(25)
            .ToList();

    public void RemoveAllRefreshTokens() => _refreshTokens.Clear();
}