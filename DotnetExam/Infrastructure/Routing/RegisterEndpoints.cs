namespace DotnetExam.Infrastructure.Routing;

public static class RegisterEndpoints
{
    public static void UseCustomEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var endpointsRoots = typeof(Program).Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IEndpointRoot)))
            .Where(t => t is { IsInterface: false, IsAbstract: false });
        
        foreach (var endpointRoot in endpointsRoots)
        {
            var instance = (IEndpointRoot)ActivatorUtilities.CreateInstance(routeBuilder.ServiceProvider, endpointRoot);
            instance?.MapEndpoints(routeBuilder);
        }
    }
    
     public static IEndpointRouteBuilder AddEndpoint<T>(this IEndpointRouteBuilder routeBuilder)
            where T : IEndpoint
        {
            var instance = ActivatorUtilities.CreateInstance<T>(routeBuilder.ServiceProvider);
            instance.Map(routeBuilder);
            return routeBuilder;
        }
}