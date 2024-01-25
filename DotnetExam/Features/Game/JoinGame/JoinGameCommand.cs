using DotnetExam.Infrastructure.Mediator.Command;

namespace DotnetExam.Features.Game.JoinGame;

public record JoinGameCommand(Guid UserId, Guid GameId, JoinMode JoinMode) : ICommand<JoinGameResponse>;

public enum JoinMode
{
    AsPlayer,
    AsViewer
}