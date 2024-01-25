using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Features.Game.Move;

public class MoveCommandHandler(IExamDbContext dbContext) : ICommandHandler<MoveCommand, MoveResponse>
{
    public async Task<MoveResponse> Handle(MoveCommand request, CancellationToken cancellationToken)
    {
        var game = await dbContext.Games
            .Include(g => g.Host.User)
            .Include(g => g.Opponent!.User)
            .FirstOrNotFoundAsync(
                g => (g.Host.UserId == request.UserId || g.Opponent!.UserId == request.UserId)
                     && g.State == GameState.Started, cancellationToken);

        var player = game.Host.UserId == request.UserId ? game.Host : game.Opponent!;

        if (!game.IsPlayerTurn(player.Mark))
        {
            throw new DomainException("Wrong turn");
        }

        if (!game.Board.IsInBound(request.X, request.Y) || game.Board.GetMark(request.X, request.Y) is not null)
        {
            throw new DomainException("Wrong place");
        }
        
        game.Move(request.X, request.Y, player.Mark);
        dbContext.Games.Update(game);
        await dbContext.SaveEntitiesAsync();

        return new MoveResponse(game.Id, game.State, game.NextTurn());
    }
}