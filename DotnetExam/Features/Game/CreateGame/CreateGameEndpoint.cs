using DotnetExam.Infrastructure.Routing;
using DotnetExam.Services.Interfaces;
using MediatR;

namespace DotnetExam.Features.Game.CreateGame;

public class CreateGameEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/create", async (IMediator mediator, IUserService userService) =>
                Results.Ok(await mediator.Send(
                    new CreateGameCommand(
                        userService.GetUserIdOrThrow()))))
            .RequireAuthorization();
    }
}