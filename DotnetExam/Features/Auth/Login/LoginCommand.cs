using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Responses;

namespace DotnetExam.Features.Auth.Login;

public record LoginCommand(string Login, string Password) : ICommand<AuthorizationResponse>;