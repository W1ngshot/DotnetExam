namespace DotnetExam.Features.Game.GetGames;

public record GameResponse(Guid GameId, DateTimeOffset CreatedAt, string HostName);

public record GetGamesResponse(List<GameResponse> Games);