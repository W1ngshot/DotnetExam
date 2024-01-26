using DotnetExam.Infrastructure.Routing;
using MediatR;

namespace DotnetExam.Features.Game.GetRatingTop;

public class GetRatingTopEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/rating", async (int count, IMediator mediator) =>
                Results.Ok(await mediator.Send(
                    new GetRatingTopQuery(count))))
            .RequireAuthorization();
    }
}