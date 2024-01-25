using DotnetExam.Infrastructure.Routing;
using DotnetExam.Infrastructure.ValidationSetup;
using MediatR;

namespace DotnetExam.Features.Auth.RefreshTokens;

public class RefreshTokensEndpoint : IEndpoint
{
    public record RefreshTokensDto(string Token, string RefreshToken);
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/refresh-tokens",
                async (RefreshTokensDto dto, IMediator mediator) =>
                    Results.Ok(await mediator.Send(new RefreshTokensCommand(
                        dto.Token,
                        dto.RefreshToken))))
            .AddValidation(builder => builder.AddFor<RefreshTokensDto>());
    }
}