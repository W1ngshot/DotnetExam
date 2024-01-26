using DotnetExam.Services;
using DotnetExam.Services.Configs;
using DotnetExam.Services.Interfaces;

namespace DotnetExam.Bootstrap;

public static class HelperServicesBootstrap
{
    public static IServiceCollection
        AddHelperServices(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>()
            .AddScoped<IDateTimeProvider, DateTimeProvider>()
            .AddScoped<AuthenticationService>()
            .AddScoped<IRandomService, RandomService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IEventSenderService, EventSenderService>()
            .AddScoped<IRatingChangeService, RatingChangeService>()
            .Configure<MongoDbConfig>(
                configuration.GetRequiredSection("MongoConfig"))
            .AddSingleton<RatingService>();
}