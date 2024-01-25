using DotnetExam.Models.Enums;

namespace DotnetExam.Features.Game.Move;

public record MoveResponse(Guid GameId, GameState State, Mark NextTurn);