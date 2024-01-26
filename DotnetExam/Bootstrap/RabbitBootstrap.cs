using DotnetExam.Services.Configs;
using MassTransit;

namespace DotnetExam.Bootstrap;

public static class RabbitBootstrap
{
    public static IServiceCollection AddMassTransitRabbit(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitConfig = new RabbitConfig();
        configuration.GetSection(RabbitConfig.Rabbit).Bind(rabbitConfig);
        return services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration.GetConnectionString("RabbitMQ")!));
                cfg.ConfigureEndpoints(context);
            });
            options.AddConsumers(typeof(Program).Assembly);
        });
    }
}