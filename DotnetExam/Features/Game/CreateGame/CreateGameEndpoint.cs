using DotnetExam.Infrastructure.Routing;
using DotnetExam.Services.Interfaces;
using MediatR;

namespace DotnetExam.Features.Game.CreateGame;

public class CreateGameEndpoint : IEndpoint
{
    private record CreateGameDto(int? MaxRating);

    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/create", async (CreateGameDto dto, IMediator mediator, IUserService userService) =>
                Results.Ok(await mediator.Send(
                    new CreateGameCommand(
                        userService.GetUserIdOrThrow(), dto.MaxRating))))
            .RequireAuthorization();
    }
}