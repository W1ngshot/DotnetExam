using DotnetExam.Infrastructure.Routing;
using DotnetExam.Services.Interfaces;
using MediatR;

namespace DotnetExam.Features.Game.RestartGame;

public class RestartGameEndpoint : IEndpoint
{
    private record RestartGameDto(Guid OpponentId, Guid OldGameId);

    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/restart", async (RestartGameDto dto, IMediator mediator, IUserService userService) =>
                Results.Ok(await mediator.Send(
                    new RestartGameCommand(
                        userService.GetUserIdOrThrow(), dto.OpponentId, dto.OldGameId))))
            .RequireAuthorization();
    }
}