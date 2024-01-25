using DotnetExam.Infrastructure.Routing;
using DotnetExam.Services.Interfaces;
using MediatR;

namespace DotnetExam.Features.Game.SendMessage;

public class SendMessageEndpoint : IEndpoint
{
    private record SendMessageDto(Guid GameId, string Message, string IdempotenceKey);

    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/send", async (SendMessageDto dto, IMediator mediator, IUserService userService) =>
                Results.Ok(await mediator.Send(
                    new SendMessageCommand(
                        userService.GetUserIdOrThrow(), dto.GameId, dto.Message, dto.IdempotenceKey))))
            .RequireAuthorization();
    }
}