using DotnetExam.Infrastructure.Mediator.Command;

namespace DotnetExam.Features.Game.Connect;

public record ConnectCommand(Guid GameId) : ICommand<ConnectResponse>;