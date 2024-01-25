using System.Security.Claims;
using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Main;
using DotnetExam.Models.Responses;
using DotnetExam.Services;
using DotnetExam.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DotnetExam.Features.Auth.RefreshTokens;

public class RefreshTokensCommandHandler(
    UserManager<AppUser> userManager,
    IExamDbContext context,
    IJwtTokenGenerator jwtTokenGenerator,
    AuthenticationService authService)
    : ICommandHandler<RefreshTokensCommand, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        var validationParamsIgnoringTime = jwtTokenGenerator.CloneParameters();
        validationParamsIgnoringTime.ValidateLifetime = false;

        var userId =
            Guid.Parse(
                jwtTokenGenerator.ReadToken(request.Token, validationParamsIgnoringTime)
                    .FindFirst(x => x.Type == ClaimTypes.NameIdentifier)
                    ?.Value
                ?? throw new UnauthorizedException("INVALID_OLD_TOKEN"));

        var applicationUser = await userManager.FindByIdAsync(userId.ToString())
                              ?? throw new NotFoundException<AppUser>();
        
        var token = applicationUser.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken)
                    ?? throw new UnauthorizedException("INVALID_REFRESH_TOKEN");

        applicationUser.RemoveRefreshToken(token);
        
        var result = await authService.AuthenticateUser(applicationUser);

        await context.SaveEntitiesAsync();
        
        return new RefreshTokenResponse(result.Token, result.RefreshToken.Token);
    }
}