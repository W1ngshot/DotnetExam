using DotnetExam.Database.Postgres.Configurations;
using DotnetExam.Database.Postgres.Configurations.Abstractions;

namespace DotnetExam.Database.Postgres;

public static class ConfigurationBootstrap
{
    public static IServiceCollection AddDatabaseConfigurations(this IServiceCollection services)
    {
        services.AddSingleton<DependencyInjectedEntityConfiguration, AppUserConfiguration>();
        services.AddSingleton<DependencyInjectedEntityConfiguration, PlayerConfiguration>();
        services.AddSingleton<DependencyInjectedEntityConfiguration, GameConfiguration>();
        
        return services;
    }
}