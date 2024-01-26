using DotnetExam.Features.Auth.AuthCheck;
using DotnetExam.Features.Auth.Login;
using DotnetExam.Features.Auth.RefreshTokens;
using DotnetExam.Features.Auth.Register;
using DotnetExam.Features.Auth.UserProfile;
using DotnetExam.Infrastructure.Routing;

namespace DotnetExam.Features.Auth;

public class AuthEndpointRoot : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/api/auth")
            .WithTags("Authorization")
            .AddEndpoint<LoginEndpoint>()
            .AddEndpoint<RegisterEndpoint>()
            .AddEndpoint<RefreshTokensEndpoint>()
            .AddEndpoint<AuthCheckEndpoint>()
            .AddEndpoint<GetProfileEndpoint>();
    }
}