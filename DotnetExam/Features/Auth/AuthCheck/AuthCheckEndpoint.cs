using DotnetExam.Infrastructure.Routing;
using DotnetExam.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotnetExam.Features.Auth.AuthCheck;

public class AuthCheckEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/get-current-id",
                ([FromServices] IUserService userService) =>
                    Results.Ok(userService.GetUserIdOrThrow()))
            .RequireAuthorization();
    }
}