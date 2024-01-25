using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Events;
using DotnetExam.Models.Main;
using DotnetExam.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DotnetExam.Features.Game.RestartGame;

public class RestartGameCommandHandler(
    IExamDbContext dbContext,
    IDateTimeProvider dateTimeProvider,
    IRandomService randomService,
    UserManager<AppUser> userManager,
    IEventSenderService eventSenderService)
    : ICommandHandler<RestartGameCommand, RestartGameResponse>
{
    public async Task<RestartGameResponse> Handle(RestartGameCommand request, CancellationToken cancellationToken)
    {
        var opponent = await dbContext.Players
            .FirstOrNotFoundAsync(player => player.Id == request.OpponentId, cancellationToken);

        if (await dbContext.Games.IsPlayingAsync(request.HostId, cancellationToken)
            || await dbContext.Games.IsPlayingAsync(opponent.UserId, cancellationToken))
        {
            throw new DomainException("Already playing");
        }

        var game = new Models.Main.Game
        {
            Host = new Player
            {
                UserId = request.HostId,
                Mark = randomService.GetRandomEnum<Mark>()
            },
            Board = new Board(),
            State = GameState.Started,
            CreatedAt = dateTimeProvider.UtcNow
        };

        var opponentPlayer = new Player
        {
            UserId = opponent.UserId,
            Mark = game.Host.Mark is Mark.Cross ? Mark.Nought : Mark.Cross
        };
        dbContext.Players.Add(opponentPlayer);
        game.OpponentId = opponentPlayer.Id;

        dbContext.Games.Add(game);
        await dbContext.SaveEntitiesAsync();

        var hostUser = await userManager.FindByIdAsync(game.Host.UserId.ToString());
        var opponentUser = await userManager.FindByIdAsync(opponentPlayer.UserId.ToString());

        var hostInfo = new PlayerInfo(game.Host.Id, hostUser!.UserName!, 0, game.Host.Mark);
        var opponentInfo = new PlayerInfo(game.Opponent!.Id, opponentUser!.UserName!, 0, game.Opponent.Mark);

        await eventSenderService.SendGameRestartEvent(
            new GameRestartEvent(request.OldGameId, game.Id, hostInfo, opponentInfo));

        return new RestartGameResponse(game.Id, hostInfo, opponentInfo, game.Board.ToStringArray());
    }
}