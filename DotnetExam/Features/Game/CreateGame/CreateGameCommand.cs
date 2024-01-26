using DotnetExam.Infrastructure.Mediator.Command;

namespace DotnetExam.Features.Game.CreateGame;

public record CreateGameCommand(Guid UserId, int? MaxRating) : ICommand<CreateGameResponse>;