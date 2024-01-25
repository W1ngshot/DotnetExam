namespace DotnetExam.Bootstrap;

public static class MediatrBootstrap
{
    public static IServiceCollection AddMediatrConfiguration(this IServiceCollection services) =>
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());
}