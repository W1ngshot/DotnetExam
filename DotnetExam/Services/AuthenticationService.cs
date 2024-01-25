using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Models.Main;
using DotnetExam.Models.Responses;
using DotnetExam.Services.Configs;
using DotnetExam.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DotnetExam.Services;

public class AuthenticationService(
    IJwtTokenGenerator jwtTokenGenerator,
    SignInManager<AppUser> signInManager,
    UserManager<AppUser> userManager)
{
    public async Task<AuthenticationResponse> AuthenticateUser(AppUser identityUser)
    {
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();
        var token = jwtTokenGenerator.GenerateUserToken(identityUser,
            DateTime.UtcNow.AddSeconds(TokensConfig.TokenLifetime));

        identityUser.AddRefreshToken(refreshToken);

        return new AuthenticationResponse(token, refreshToken);
    }
    
    public async Task<AuthenticationResponse> ProcessPasswordLogin(string login, string password)
    {
        var identityUser = await userManager.FindByNameAsync(login) ?? throw new NotFoundException<AppUser>();
        
        var result = await signInManager.CheckPasswordSignInAsync(identityUser, password, false);

        if (!result.Succeeded)
            throw new ValidationFailedException("Password", "Wrong password");

        return await AuthenticateUser(identityUser);
    }
}