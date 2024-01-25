using DotnetExam.Infrastructure.Mediator.Query;

namespace DotnetExam.Features.Game.GetGames;

public record GetGamesQuery(int Skip, int Count) : IQuery<GetGamesResponse>;