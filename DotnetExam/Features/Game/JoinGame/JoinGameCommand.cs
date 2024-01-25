using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Responses;

namespace DotnetExam.Features.Game.JoinGame;

public record JoinGameCommand(Guid UserId, Guid GameId) : ICommand<SuccessResponse>;