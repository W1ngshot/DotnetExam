using DotnetExam.Infrastructure.Routing;
using MediatR;

namespace DotnetExam.Features.Game.GetGames;

public class GetGamesEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/", async (int skip, int count, IMediator mediator) =>
            Results.Ok(await mediator.Send(
                new GetGamesQuery(skip, count))))
            .RequireAuthorization();
    }
}