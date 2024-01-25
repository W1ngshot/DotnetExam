namespace DotnetExam.Services.Interfaces;

public interface IUserService
{
    public Guid GetUserIdOrThrow();
}