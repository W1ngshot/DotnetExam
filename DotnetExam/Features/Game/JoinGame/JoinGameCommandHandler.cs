using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Events;
using DotnetExam.Models.Main;
using DotnetExam.Services;
using DotnetExam.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Features.Game.JoinGame;

public class JoinGameCommandHandler(
    IExamDbContext dbContext,
    UserManager<AppUser> userManager,
    IEventSenderService eventSenderService,
    RatingService ratingService)
    : ICommandHandler<JoinGameCommand, JoinGameResponse>
{
    public async Task<JoinGameResponse> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        var game = await dbContext.Games
            .Include(game => game.Host.User)
            .Include(game => game.Opponent!.User)
            .FirstOrNotFoundAsync(game => game.Id == request.GameId, cancellationToken);

        if (game.Opponent is not null && game.Opponent.UserId == request.UserId || game.Host.UserId == request.UserId)
        {
            var hostInformation = new PlayerInfo(game.Host.UserId, game.Host.User.UserName!,
                await ratingService.GetUserRatingAsync(game.Host.UserId), game.Host.Mark);
            if (game.Opponent is null)
            {
                return new JoinGameResponse(game.Id, hostInformation, null, game.Board.ToStringArray(),
                    null, game.State);
            }

            var opponentInformation = new PlayerInfo(game.Opponent!.UserId, game.Opponent.User.UserName!,
                await ratingService.GetUserRatingAsync(game.Opponent.UserId), game.Opponent.Mark);
            return new JoinGameResponse(game.Id, hostInformation, opponentInformation, game.Board.ToStringArray(),
                GetNextTurnId(game),
                game.State);
        }
        
        if (await dbContext.Games.IsPlayingAsync(request.UserId, cancellationToken))
        {
            throw new BadRequestException("Already playing");
        }

        if (request.JoinMode is JoinMode.AsViewer)
        {
            return await JoinAsViewer(game);
        }

        if (await ratingService.GetUserRatingAsync(request.UserId) > game.MaxRating)
        {
            throw new BadRequestException("Wrong rating");
        }

        if (game.State is not GameState.NotStarted || game.Opponent is not null)
        {
            throw new BadRequestException("Already started");
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

        var hostInfo = new PlayerInfo(game.Host.UserId, game.Host.User.UserName!,
            await ratingService.GetUserRatingAsync(game.Host.UserId), game.Host.Mark);
        var opponentInfo = new PlayerInfo(game.Opponent!.UserId, opponentUser!.UserName!,
            await ratingService.GetUserRatingAsync(game.Opponent.UserId), game.Opponent.Mark);

        await eventSenderService.SendGameStartEvent(
            new GameStartEvent(game.Id, hostInfo, opponentInfo, game.Board.ToStringArray(), GetNextTurnId(game),
                game.State));

        return new JoinGameResponse(game.Id, hostInfo, opponentInfo, game.Board.ToStringArray(), GetNextTurnId(game),
            game.State);
    }

    private async Task<JoinGameResponse> JoinAsViewer(Models.Main.Game game)
    {
        var hostInfo = new PlayerInfo(game.Host.UserId, game.Host.User.UserName!,
            await ratingService.GetUserRatingAsync(game.Host.UserId), game.Host.Mark);

        if (game.Opponent is null)
        {
            return new JoinGameResponse(game.Id, hostInfo, null, game.Board.ToStringArray(), GetNextTurnId(game),
                game.State);
        }

        var opponentUser = await userManager.FindByIdAsync(game.Opponent.UserId.ToString());
        var opponentInfo = new PlayerInfo(game.Opponent!.UserId, opponentUser!.UserName!,
            await ratingService.GetUserRatingAsync(game.Opponent.UserId), game.Opponent.Mark);
        return new JoinGameResponse(game.Id, hostInfo, opponentInfo, game.Board.ToStringArray(), GetNextTurnId(game),
            game.State);
    }

    private Guid GetNextTurnId(Models.Main.Game game) =>
        game.Host.Mark == game.NextTurn() ? game.Host.Id : game.Opponent!.Id;
}