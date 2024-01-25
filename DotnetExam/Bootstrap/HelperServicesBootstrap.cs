using DotnetExam.Services;
using DotnetExam.Services.Interfaces;

namespace DotnetExam.Bootstrap;

public static class HelperServicesBootstrap
{
    public static IServiceCollection AddHelperServices(this IServiceCollection servicesCollection) =>
        servicesCollection
            .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>()
            .AddScoped<IDateTimeProvider, DateTimeProvider>()
            .AddScoped<AuthenticationService>()
            .AddScoped<IRandomService, RandomService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IEventSenderService, EventSenderService>();
}