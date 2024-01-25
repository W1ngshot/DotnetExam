namespace DotnetExam.Models.Responses;

public record AuthorizationResponse(string Token, string RefreshToken)
{
    public static AuthorizationResponse FromAuthenticationResult(AuthenticationResponse authenticationResponse)
    {
        return new AuthorizationResponse(authenticationResponse.Token, authenticationResponse.RefreshToken.Token);
    }
}