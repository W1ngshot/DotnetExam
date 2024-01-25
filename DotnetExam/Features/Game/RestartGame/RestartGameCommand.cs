using DotnetExam.Infrastructure.Mediator.Command;

namespace DotnetExam.Features.Game.RestartGame;

public record RestartGameCommand(Guid HostId, Guid OpponentId, Guid OldGameId) : ICommand<RestartGameResponse>;