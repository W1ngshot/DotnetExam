using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Main;
using DotnetExam.Services.Interfaces;

namespace DotnetExam.Features.Game.CreateGame;

public class CreateGameCommandHandler(
    IExamDbContext dbContext,
    IDateTimeProvider dateTimeProvider,
    IRandomService randomService)
    : ICommandHandler<CreateGameCommand, CreateGameResponse>
{
    public async Task<CreateGameResponse> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Games.IsPlayingAsync(request.UserId, cancellationToken))
        {
            throw new DomainException("Already playing");
        }

        var game = new Models.Main.Game
        {
            Host = new Player
            {
                UserId = request.UserId,
                Mark = randomService.GetRandomEnum<Mark>()
            },
            Board = new Board(),
            State = GameState.NotStarted,
            CreatedAt = dateTimeProvider.UtcNow
        };

        dbContext.Games.Add(game);
        await dbContext.SaveEntitiesAsync();

        return new CreateGameResponse(game.Id, game.Host.Mark);
    }
}