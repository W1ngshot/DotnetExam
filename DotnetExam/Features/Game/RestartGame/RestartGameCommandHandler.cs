using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Main;
using DotnetExam.Services.Interfaces;

namespace DotnetExam.Features.Game.RestartGame;

public class RestartGameCommandHandler(
    IExamDbContext dbContext,
    IDateTimeProvider dateTimeProvider,
    IRandomService randomService) 
    : ICommandHandler<RestartGameCommand, RestartGameResponse>
{
    public async Task<RestartGameResponse> Handle(RestartGameCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Games.IsPlayingAsync(request.HostId, cancellationToken)
            || await dbContext.Games.IsPlayingAsync(request.OpponentId, cancellationToken))
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
        
        var opponent = new Player
        {
            UserId = request.OpponentId,
            Mark = game.Host.Mark is Mark.Cross ? Mark.Nought : Mark.Cross
        };
        dbContext.Players.Add(opponent);
        game.OpponentId = opponent.Id;

        dbContext.Games.Add(game);
        await dbContext.SaveEntitiesAsync();

        return new RestartGameResponse(game.Id, game.Host.Id, game.Host.User.UserName!, game.Opponent?.UserId,
            game.Opponent?.User.UserName, game.State, game.Board, game.Host.Mark);
    }
}