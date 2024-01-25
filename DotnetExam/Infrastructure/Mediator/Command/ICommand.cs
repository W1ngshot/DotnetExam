using MediatR;

namespace DotnetExam.Infrastructure.Mediator.Command;

public interface ICommand<out T> : IRequest<T>
{
    
}