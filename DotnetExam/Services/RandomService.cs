using DotnetExam.Services.Interfaces;

namespace DotnetExam.Services;

public class RandomService : IRandomService
{
    private readonly Random _random = Random.Shared;
    
    public TEnum GetRandomEnum<TEnum>() where TEnum : struct, Enum
    {
        var enums = Enum.GetValues<TEnum>();
        return enums[_random.Next(enums.Length)];
    }
}