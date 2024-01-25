﻿using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Events;
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
        
        SendEvent(game, request.IdempotenceKey, player.Id);
        
        return new MoveResponse(game.Id, game.Board.ToStringArray(), player.Id, request.IdempotenceKey);
    }

    private void SendEvent(Models.Main.Game game, string idempotenceKey, Guid currentId)
    {
        if (game.State == GameState.Started)
        {
            var moveGameEvent = new PlayerMoveEvent(game.Id, idempotenceKey, game.Board.ToStringArray(),
                currentId == game.Host.Id ? game.Opponent!.Id : game.Host.Id);
            return;
        }

        var gameOverEvent = new GameOverEvent(game.Id, game.Board.ToStringArray(), currentId, game.State);
    }
}