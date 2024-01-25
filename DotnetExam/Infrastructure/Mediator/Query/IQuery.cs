using MediatR;

namespace DotnetExam.Infrastructure.Mediator.Query;

public interface IQuery<out T> : IRequest<T>
{

}