using System.Net;

namespace DotnetExam.Infrastructure.Exceptions;

public class ForbiddenException: DomainException
{
    public ForbiddenException(string message) : base(message, (int)HttpStatusCode.Forbidden)
    {
    }
}
