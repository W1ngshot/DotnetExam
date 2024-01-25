using System.Net;

namespace DotnetExam.Infrastructure.Exceptions;

public class NotFoundException<T> : DomainException
{
    public NotFoundException() : base(
        ErrorMessages.NotFound, (int)HttpStatusCode.NotFound)
    {
        PlaceholderData.Add("EntityName", typeof(T).Name);
    }
}