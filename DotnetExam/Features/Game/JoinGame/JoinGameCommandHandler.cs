using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Events;
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
        {
            return await JoinAsViewer(game);
        }

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
        
        var hostInfo = new PlayerInfo(game.Host.Id, game.Host.User.UserName!, 0, game.Host.Mark);
        var opponentInfo = new PlayerInfo(game.Opponent!.Id, opponentUser!.UserName!, 0, game.Opponent.Mark);
        var gameStartEvent = new GameStartEvent(game.Id, hostInfo, opponentInfo);

        return new JoinGameResponse(game.Id, hostInfo, opponentInfo, game.Board.ToStringArray(), game.NextTurn());
    }

    private async Task<JoinGameResponse> JoinAsViewer(Models.Main.Game game)
    {
        var hostInfo = new PlayerInfo(game.Host.Id, game.Host.User.UserName!, 0, game.Host.Mark);
        
        if (game.Opponent is null)
        {
            return new JoinGameResponse(game.Id, hostInfo, null, game.Board.ToStringArray(), game.NextTurn());
        }
        
        var opponentUser = await userManager.FindByIdAsync(game.Opponent.UserId.ToString());
        var opponentInfo = new PlayerInfo(game.Opponent!.Id, opponentUser!.UserName!, 0, game.Opponent.Mark);
        return new JoinGameResponse(game.Id, hostInfo, opponentInfo, game.Board.ToStringArray(), game.NextTurn());
    }
}