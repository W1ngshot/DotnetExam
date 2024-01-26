using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Events;
using DotnetExam.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Features.Game.Move;

public class MoveCommandHandler(
    IExamDbContext dbContext,
    IEventSenderService eventSenderService,
    IRatingChangeService ratingChangeService)
    : ICommandHandler<MoveCommand, MoveResponse>
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
            throw new BadRequestException("Wrong turn");
        }

        if (!game.Board.IsInBound(request.X, request.Y) || game.Board.GetMark(request.X, request.Y) is not null)
        {
            throw new BadRequestException("Wrong place");
        }

        game.Move(request.X, request.Y, player.Mark);
        dbContext.Games.Update(game);
        await dbContext.SaveEntitiesAsync();

        await SendEvent(game, request.IdempotenceKey, player.UserId);
        return new MoveResponse(game.Id, game.Board.ToStringArray(), player.UserId, request.IdempotenceKey);
    }

    private async Task SendEvent(Models.Main.Game game, string idempotenceKey, Guid currentId)
    {
        if (game.State == GameState.Started)
        {
            await eventSenderService.SendPlayerMoveEvent(
                new PlayerMoveEvent(game.Id, idempotenceKey, game.Board.ToStringArray(),
                    currentId == game.Host.UserId ? game.Opponent!.UserId : game.Host.UserId));
            return;
        }

        if (game.State != GameState.Draw)
        {
            await ratingChangeService.SendRatingChange(
                game.Host.Id == currentId ? game.Host.UserId : game.Opponent!.UserId,
                game.Host.Id == currentId ? game.Opponent!.UserId : game.Host.UserId);
        }

        await eventSenderService.SendGameOverEvent(
            new GameOverEvent(game.Id, game.Board.ToStringArray(),
                game.State == GameState.Draw ? null : currentId,
                game.State));
    }
}