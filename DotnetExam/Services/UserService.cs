using System.Security.Claims;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Services.Interfaces;

namespace DotnetExam.Services;


public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
{
    private Guid? UserId => Guid.TryParse(
        httpContextAccessor.HttpContext
            ?.User
            .FindFirstValue(ClaimTypes.NameIdentifier),
        out var userId)
        ? userId
        : null;

    public Guid GetUserIdOrThrow() => UserId ?? throw new UnauthorizedException("Unauthorized");
}