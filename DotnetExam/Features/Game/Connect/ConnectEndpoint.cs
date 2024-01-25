using DotnetExam.Infrastructure.Routing;
using MediatR;

namespace DotnetExam.Features.Game.Connect;

public class ConnectEndpoint : IEndpoint
{
    public record ConnectDto(Guid GameId);
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/connect", async (ConnectDto dto, IMediator mediator) =>
                Results.Ok(await mediator.Send(
                    new ConnectCommand(dto.GameId))))
            .RequireAuthorization();
    }
}