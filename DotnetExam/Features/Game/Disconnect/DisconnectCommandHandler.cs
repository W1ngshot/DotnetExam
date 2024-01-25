﻿using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Features.Game.Disconnect;

public class DisconnectCommandHandler(IExamDbContext dbContext) : ICommandHandler<DisconnectCommand, DisconnectResponse?>
{
    public async Task<DisconnectResponse?> Handle(DisconnectCommand request, CancellationToken cancellationToken)
    {
        var game = await dbContext.Games
            .Include(g => g.Host.User)
            .Include(g => g.Opponent!.User)
            .FirstOrDefaultAsync(
                g => (g.Host.UserId == request.UserId || g.Opponent != null && g.Opponent!.UserId == request.UserId)
                     && (g.State == GameState.NotStarted || g.State == GameState.Started), cancellationToken);

        if (game is null)
        {
            return null;
        }
        
        if (game.State is not GameState.Started)
        {
            game.State = GameState.Draw;
            await dbContext.SaveEntitiesAsync();

            return new DisconnectResponse(false, null, game.State, game.Id);
        }

        var winner = game.Host.UserId == request.UserId ? game.Opponent : game.Host;
        game.State = winner!.Mark is Mark.Cross ? GameState.CrossesWon : GameState.NoughtsWon;

        await dbContext.SaveEntitiesAsync();

        return new DisconnectResponse(true, winner.UserId, game.State, game.Id);
    }
}