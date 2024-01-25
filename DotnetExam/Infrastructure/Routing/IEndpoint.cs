namespace DotnetExam.Infrastructure.Routing;

public interface IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints);
}