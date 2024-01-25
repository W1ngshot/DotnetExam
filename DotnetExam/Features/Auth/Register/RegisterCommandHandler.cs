using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Main;
using DotnetExam.Models.Responses;
using DotnetExam.Services;
using Microsoft.AspNetCore.Identity;

namespace DotnetExam.Features.Auth.Register;

public class RegisterCommandHandler(
    UserManager<AppUser> userManager,
    AuthenticationService authService,
    IExamDbContext dbContext)
    : ICommandHandler<RegisterCommand, AuthorizationResponse>
{
    public async Task<AuthorizationResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var identityUser = new AppUser
        {
            UserName = command.Login
        };
        var result = await userManager.CreateAsync(identityUser, command.Password);

        if (!result.Succeeded)
            throw new UnauthorizedException(string.Join("\n", result.Errors.Select(x => x.Description)));

        var tokens = await authService.AuthenticateUser(identityUser);
        await dbContext.SaveEntitiesAsync();
        return AuthorizationResponse.FromAuthenticationResult(tokens);
    }
}
