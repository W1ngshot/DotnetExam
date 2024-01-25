using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Events;
using DotnetExam.Models.Responses;
using DotnetExam.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Features.Game.Disconnect;

public class DisconnectCommandHandler(IExamDbContext dbContext, IEventSenderService eventSenderService)
    : ICommandHandler<DisconnectCommand, SuccessResponse>
{
    public async Task<SuccessResponse> Handle(DisconnectCommand request, CancellationToken cancellationToken)
    {
        var game = await dbContext.Games
            .Include(g => g.Host.User)
            .Include(g => g.Opponent!.User)
            .FirstOrDefaultAsync(
                g => (g.Host.UserId == request.UserId || g.Opponent != null && g.Opponent!.UserId == request.UserId)
                     && (g.State == GameState.NotStarted || g.State == GameState.Started), cancellationToken);

        if (game is null)
        {
            return new SuccessResponse();
        }

        if (game.State is not GameState.Started)
        {
            game.State = GameState.Draw;
            await dbContext.SaveEntitiesAsync();
            await eventSenderService.SendGameOverEvent(CreateGameOverEvent(game, null));
            return new SuccessResponse();
        }

        var winner = game.Host.UserId == request.UserId ? game.Opponent : game.Host;
        game.State = winner!.Mark is Mark.Cross ? GameState.CrossesWon : GameState.NoughtsWon;
        await dbContext.SaveEntitiesAsync();

        await eventSenderService.SendGameOverEvent(CreateGameOverEvent(game, winner.Id));
        return new SuccessResponse();
    }

    private static GameOverEvent CreateGameOverEvent(Models.Main.Game game, Guid? winnerId)
    {
        return new GameOverEvent(game.Id, game.Board.ToStringArray(), winnerId, game.State);
    }
}