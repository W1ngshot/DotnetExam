using DotnetExam.Infrastructure.Routing;
using DotnetExam.Infrastructure.ValidationSetup;
using MediatR;

namespace DotnetExam.Features.Auth.Login;

public class LoginEndpoint : IEndpoint
{
    public record LoginDto(string Login, string Password);

    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/login", async (LoginDto dto, IMediator mediator) =>
                Results.Ok(await mediator.Send(new LoginCommand(
                    dto.Login,
                    dto.Password))))
            .AddValidation(builder => builder.AddFor<LoginDto>());
        
    }
}