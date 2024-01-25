using DotnetExam.Infrastructure.Mediator.Command;

namespace DotnetExam.Features.Game.Move;

public record MoveCommand(Guid UserId, int X, int Y, string IdempotenceKey) : ICommand<MoveResponse>;