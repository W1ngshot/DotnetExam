using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Main;
using DotnetExam.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Features.Game.JoinGame;

public class JoinGameCommandHandler(IExamDbContext dbContext) : ICommandHandler<JoinGameCommand, SuccessResponse>
{
    public async Task<SuccessResponse> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Games.IsPlayingAsync(request.UserId, cancellationToken))
        {
            throw new DomainException("Already playing");
        }

        var game = await dbContext.Games
            .Include(game => game.Host)
            .FirstOrNotFoundAsync(game => game.Id == request.GameId, cancellationToken);

        if (game.State is not GameState.NotStarted && game.Opponent is not null)
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

        return new SuccessResponse();
    }
}