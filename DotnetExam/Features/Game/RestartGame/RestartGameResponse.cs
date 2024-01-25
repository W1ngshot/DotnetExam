using DotnetExam.Models;

namespace DotnetExam.Features.Game.RestartGame;

public record RestartGameResponse(
    Guid GameId,
    PlayerInfo Host,
    PlayerInfo? Opponent,
    string[] Board);