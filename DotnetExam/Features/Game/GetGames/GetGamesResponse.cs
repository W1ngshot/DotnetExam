using DotnetExam.Models;
using DotnetExam.Models.Enums;

namespace DotnetExam.Features.Game.GetGames;

public record GameResponse(
    Guid GameId,
    DateTimeOffset CreatedAt,
    GameState State,
    PlayerInfo Host,
    PlayerInfo? Opponent);

public record GetGamesResponse(List<GameResponse> Games);