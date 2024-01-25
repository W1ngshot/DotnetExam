using DotnetExam.Infrastructure.Routing;
using DotnetExam.Services.Interfaces;
using MediatR;

namespace DotnetExam.Features.Game.JoinGame;

public class JoinGameEndpoint : IEndpoint
{
    private record JoinGameDto(Guid GameId, JoinMode JoinMode);
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/join", async (JoinGameDto dto, IMediator mediator, IUserService userService) =>
                Results.Ok(await mediator.Send(
                    new JoinGameCommand(
                        userService.GetUserIdOrThrow(), dto.GameId, dto.JoinMode))))
            .RequireAuthorization();
    }
}