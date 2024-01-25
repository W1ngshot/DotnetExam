using DotnetExam.Models.Enums;

namespace DotnetExam.Features.Game.Move;

public record MoveResponse(Guid GameId, int X, int Y, Mark Mark, GameState State);