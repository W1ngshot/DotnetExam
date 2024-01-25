using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Main;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Features.Game.JoinGame;

public class JoinGameCommandHandler(IExamDbContext dbContext, UserManager<AppUser> userManager)
    : ICommandHandler<JoinGameCommand, JoinGameResponse>
{
    public async Task<JoinGameResponse> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Games.IsPlayingAsync(request.UserId, cancellationToken))
        {
            throw new DomainException("Already playing");
        }

        var game = await dbContext.Games
            .Include(game => game.Host.User)
            .Include(game => game.Opponent!.User)
            .FirstOrNotFoundAsync(game => game.Id == request.GameId, cancellationToken);

        if (request.JoinMode is JoinMode.AsViewer)
            return new JoinGameResponse(game.Host.Id, game.Host.User.UserName!, game.Opponent?.UserId,
                game.Opponent?.User.UserName, game.State, game.Board, null, game.NextTurn());

        if (game.State is not GameState.NotStarted || game.Opponent is not null)
        {
            throw new DomainException("Already started");
        }

        var opponent = new Player
        {
            UserId = request.UserId,
            Mark = game.Host.Mark is Mark.Cross ? Mark.Nought : Mark.Cross
        };
        
        dbContext.Players.Add(opponent);
        game.OpponentId = opponent.Id;
        game.State = GameState.Started;

        await dbContext.SaveEntitiesAsync();

        var opponentUser = await userManager.FindByIdAsync(opponent.UserId.ToString());
        return new JoinGameResponse(game.Host.Id, game.Host.User.UserName!, game.Opponent?.UserId,
            opponentUser?.UserName, game.State, game.Board, opponent.Mark, game.NextTurn());
    }
}