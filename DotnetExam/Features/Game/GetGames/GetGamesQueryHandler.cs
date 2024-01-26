using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure.Mediator.Query;
using DotnetExam.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Features.Game.GetGames;

public class GetGamesQueryHandler(IExamDbContext dbContext) : IQueryHandler<GetGamesQuery, GetGamesResponse>
{
    public async Task<GetGamesResponse> Handle(GetGamesQuery request, CancellationToken cancellationToken)
    {
        return new GetGamesResponse(
            await dbContext.Games
                .Include(game => game.Host.User)
                .Include(game => game.Opponent!.User)
                .OrderByDescending(game => game.Opponent == null)
                .ThenByDescending(game => game.CreatedAt)
                .Skip(request.Skip)
                .Take(request.Count)
                .Select(game => new GameResponse(game.Id, game.CreatedAt, game.State, game.MaxRating,
                    new PlayerInfo(game.Host.UserId, game.Host.User.UserName!, 0, game.Host.Mark),
                    game.Opponent == null
                        ? null
                        : new PlayerInfo(game.Opponent.Id, game.Opponent.User.UserName!, 0, game.Opponent.Mark)))
                .ToListAsync(cancellationToken));
    }
}