using DotnetExam.Infrastructure.Routing;
using DotnetExam.Services.Interfaces;
using MediatR;

namespace DotnetExam.Features.Auth.UserProfile;

public class GetProfileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/profile", async (IUserService userService, IMediator mediator) =>
            Results.Ok(await mediator.Send(
                new GetProfileQuery(userService.GetUserIdOrThrow()))));
    }
}