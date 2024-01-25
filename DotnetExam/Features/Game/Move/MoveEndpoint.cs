using DotnetExam.Infrastructure.Routing;
using DotnetExam.Services.Interfaces;
using MediatR;

namespace DotnetExam.Features.Game.Move;

public class MoveEndpoint : IEndpoint
{
    private record MoveDto(int X, int Y, string IdempotenceKey);
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/move", async (MoveDto dto, IMediator mediator, IUserService userService) =>
                Results.Ok(await mediator.Send(
                    new MoveCommand(
                        userService.GetUserIdOrThrow(), dto.X, dto.Y, dto.IdempotenceKey))))
            .RequireAuthorization();
    }
}