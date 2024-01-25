using FluentValidation;
using FluentValidation.AspNetCore;

namespace DotnetExam.Bootstrap;

public static class FluentValidationBootstrap
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services) => 
        services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(typeof(Program).Assembly);
}