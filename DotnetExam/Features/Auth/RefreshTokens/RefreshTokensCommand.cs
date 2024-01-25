using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Responses;

namespace DotnetExam.Features.Auth.RefreshTokens;

public record RefreshTokensCommand(string Token, string RefreshToken) : ICommand<RefreshTokenResponse>;