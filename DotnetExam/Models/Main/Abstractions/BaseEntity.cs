namespace DotnetExam.Models.Main.Abstractions;

public abstract class BaseEntity
{
    public Guid Id { get; protected init; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}