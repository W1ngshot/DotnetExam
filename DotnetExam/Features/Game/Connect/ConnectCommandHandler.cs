using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Mediator.Command;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Features.Game.Connect;

public class ConnectCommandHandler(IExamDbContext dbContext) : ICommandHandler<ConnectCommand, ConnectResponse>
{
    public async Task<ConnectResponse> Handle(ConnectCommand request, CancellationToken cancellationToken)
    {
        //TODO подгружать только при Opponent != null
        var game = await dbContext.Games
            .Include(game => game.Host.User)
            .Include(game => game.Opponent.User)
            .FirstOrNotFoundAsync(game => game.Id == request.GameId, cancellationToken);

        return new ConnectResponse(game.Host.Id, game.Host.User.UserName!, game.Opponent?.Id,
            game.Opponent?.User.UserName!, game.State);
    }
}