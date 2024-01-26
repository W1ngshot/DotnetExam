using DotnetExam.Database.Postgres.Interfaces;
using DotnetExam.Infrastructure;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Mediator.Command;
using DotnetExam.Models;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Main;
using DotnetExam.Services;
using DotnetExam.Services.Interfaces;

namespace DotnetExam.Features.Game.CreateGame;

public class CreateGameCommandHandler(
    IExamDbContext dbContext,
    IDateTimeProvider dateTimeProvider,
    IRandomService randomService,
    RatingService ratingService)
    : ICommandHandler<CreateGameCommand, CreateGameResponse>
{
    public async Task<CreateGameResponse> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Games.IsPlayingAsync(request.UserId, cancellationToken))
        {
            throw new BadRequestException("Already playing");
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
            CreatedAt = dateTimeProvider.UtcNow,
            MaxRating = request.MaxRating
        };

        dbContext.Games.Add(game);
        await dbContext.SaveEntitiesAsync();

        var host = await ratingService.GetUserInfoAsync(request.UserId);
        return new CreateGameResponse(game.Id,
            new PlayerInfo(game.Host.Id, host.Username, host.Rating, game.Host.Mark));
    }
}