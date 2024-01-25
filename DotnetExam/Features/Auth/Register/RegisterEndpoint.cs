using DotnetExam.Infrastructure.Routing;
using DotnetExam.Infrastructure.ValidationSetup;
using MediatR;

namespace DotnetExam.Features.Auth.Register;

public class RegisterEndpoint : IEndpoint
{
    public record RegisterDto(string Login, string Password);
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/register", async (RegisterDto dto, IMediator mediator) =>
                Results.Ok(await mediator.Send(new RegisterCommand(
                    dto.Login,
                    dto.Password))))
            .AddValidation(builder => builder.AddFor<RegisterDto>());
    }
}