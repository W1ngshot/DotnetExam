namespace DotnetExam.Features.Game.Move;

public record MoveResponse(Guid GameId, string[] Board, Guid CurrentTurnId, string IdempotenceKey);