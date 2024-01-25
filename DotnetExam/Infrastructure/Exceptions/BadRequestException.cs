using System.Net;

namespace DotnetExam.Infrastructure.Exceptions;

public class BadRequestException<T> : DomainException
{
    public BadRequestException() : base(
        ErrorMessages.BadRequest, (int)HttpStatusCode.BadRequest)
    {
        PlaceholderData.Add("EntityName", typeof(T).Name);
    }
}
public class BadRequestException : DomainException
{
    public BadRequestException(string message) : base(
        message, (int)HttpStatusCode.BadRequest)
    {
    }
}