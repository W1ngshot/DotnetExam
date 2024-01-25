namespace DotnetExam.Infrastructure.Routing;

public interface IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints);
}