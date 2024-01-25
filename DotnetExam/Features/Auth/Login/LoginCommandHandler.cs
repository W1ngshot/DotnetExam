using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Responses;

namespace DotnetExam.Features.Auth.Login;

public class LoginCommandHandler(IExamDbContext context, Services.AuthenticationService authService)
    : ICommandHandler<LoginCommand, AuthorizationResponse>
{
    public async Task<AuthorizationResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var result = AuthorizationResponse.FromAuthenticationResult(
            await authService.ProcessPasswordLogin(
                command.Login
                ?? throw new ValidationFailedException(nameof(command.Login), "LOGIN_IS_NULL"),
                command.Password
                ?? throw new ValidationFailedException(nameof(command.Password), "PASSWORD_IS_NULL")));

        await context.SaveEntitiesAsync();
        return result;
    }
}