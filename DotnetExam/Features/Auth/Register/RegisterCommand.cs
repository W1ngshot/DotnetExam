using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Responses;

namespace DotnetExam.Features.Auth.Register;

public record RegisterCommand(string Login, string Password) : ICommand<AuthorizationResponse>;