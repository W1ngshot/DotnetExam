using DotnetExam.Features.Game.CreateGame;
using DotnetExam.Features.Game.GetGames;
using DotnetExam.Features.Game.JoinGame;
using DotnetExam.Features.Game.Move;
using DotnetExam.Features.Game.RestartGame;
using DotnetExam.Features.Game.SendMessage;
using DotnetExam.Infrastructure.Routing;

namespace DotnetExam.Features.Game;

public class GameEndpointRoot : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/api/game")
            .WithTags("Game")
            .AddEndpoint<CreateGameEndpoint>()
            .AddEndpoint<JoinGameEndpoint>()
            .AddEndpoint<GetGamesEndpoint>()
            .AddEndpoint<RestartGameEndpoint>()
            .AddEndpoint<MoveEndpoint>()
            .AddEndpoint<SendMessageEndpoint>();
    }
}