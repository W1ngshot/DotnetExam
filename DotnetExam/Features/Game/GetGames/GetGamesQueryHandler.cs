using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure.Mediator.Query;
using DotnetExam.Models;
using DotnetExam.Services;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Features.Game.GetGames;

public class GetGamesQueryHandler(IExamDbContext dbContext, RatingService ratingService)
    : IQueryHandler<GetGamesQuery, GetGamesResponse>
{
    public async Task<GetGamesResponse> Handle(GetGamesQuery request, CancellationToken cancellationToken)
    {
        var games = await dbContext.Games
            .Include(game => game.Host.User)
            .Include(game => game.Opponent!.User)
            .OrderByDescending(game => game.Opponent == null)
            .ThenByDescending(game => game.CreatedAt)
            .Skip(request.Skip)
            .Take(request.Count)
            .ToListAsync(cancellationToken);

        var players = games
            .SelectMany(response => new[] {response.Host, response.Opponent})
            .Where(guid => guid is not null)
            .Select(playerInfo => playerInfo!.UserId)
            .ToList();

        var infos = await ratingService.GetUserListRatingAsync(players);
        return new GetGamesResponse(games.Select(game => new GameResponse(game.Id, game.CreatedAt, game.State,
                game.MaxRating,
                new PlayerInfo(game.Host.UserId, game.Host.User.UserName!,
                    infos.First(x => x.UserId == game.Host.UserId).Rating, game.Host.Mark),
                game.Opponent == null
                    ? null
                    : new PlayerInfo(game.Opponent.UserId, game.Opponent.User.UserName!,
                        infos.First(x => x.UserId == game.Opponent.UserId).Rating, game.Opponent.Mark)))
            .ToList());
    }
}