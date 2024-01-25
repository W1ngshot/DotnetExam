using DotnetExam.Infrastructure.Mediator.Command;

namespace DotnetExam.Features.Game.Disconnect;

public record DisconnectCommand(Guid UserId) : ICommand<DisconnectResponse?>;