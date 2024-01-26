using DotnetExam.Models;
using DotnetExam.Models.Enums;

namespace DotnetExam.Features.Game.JoinGame;

public record JoinGameResponse(
    Guid GameId,
    PlayerInfo Host,
    PlayerInfo? Opponent,
    string[] Board,
    Guid CurrentTurnId,
    GameState State); 