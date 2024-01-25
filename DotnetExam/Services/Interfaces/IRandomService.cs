namespace DotnetExam.Services.Interfaces;

public interface IRandomService
{
    TEnum GetRandomEnum<TEnum>() where TEnum : struct, Enum;
}