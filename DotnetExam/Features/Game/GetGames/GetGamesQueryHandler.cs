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
                .OrderByDescending(game => game.Opponent == null)
                .ThenByDescending(game => game.CreatedAt)
                .Skip(request.Skip)
                .Take(request.Count)
                .Select(game => new GameResponse(game.Id, game.CreatedAt, game.State,
                    new PlayerInfo(game.Host.UserId, game.Host.User.UserName!, 0, game.Host.Mark)))
                .ToListAsync(cancellationToken));
    }
}