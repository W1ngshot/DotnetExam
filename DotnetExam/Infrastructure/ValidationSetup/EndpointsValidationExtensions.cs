using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Infrastructure.Exceptions.Utlils;
using FluentValidation;

namespace DotnetExam.Infrastructure.ValidationSetup;

public static class EndpointsValidationExtensions
{
    public static TBuilder AddValidation<TBuilder>(this TBuilder builder, Action<ValidationBuilder<TBuilder>> config)
        where TBuilder : IEndpointConventionBuilder
    {
        var validationBuilder = new ValidationBuilder<TBuilder>(builder);
        
        config(validationBuilder);
        validationBuilder.SetValidation();
        
        return builder;
    }

    /// <summary>
    /// Добавить валидацию к указанным типам.
    /// </summary>
    /// <remarks>
    /// Получает валидаторы из DI
    /// <br />
    /// Выкидывает исключение если валидатор отсутствует
    /// </remarks>
    public static TBuilder AddValidationFilter<TBuilder>(
        this TBuilder builder,
        Type toValidate)
        where TBuilder : IEndpointConventionBuilder
        => AddValidationFilter(builder, new[] { toValidate });

    /// <summary>
    /// Добавить валидацию к указанным типам.
    /// </summary>
    /// <remarks>
    /// Получает валидаторы из DI
    /// <br />
    /// Выкидывает исключение если валидатор отсутствует
    /// </remarks>
    public static TBuilder AddValidationFilter<TBuilder>(
        this TBuilder builder,
        Type[] toValidate)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.AddEndpointFilterFactory(
            (factoryContext, next) =>
            {
                var args = factoryContext.MethodInfo.GetParameters();

                var missingArgs = toValidate
                    .Where(type => args.All(x => x.ParameterType != type))
                    .ToList();

                if (missingArgs.Count > 0)
                    throw new InvalidOperationException("Selected types are not listed in the endpoint lambda's args");

                // Заготовки собранных генериков для заданных типов
                var validatorTypes = toValidate
                    .Select(x => (Type: x, ValidatorType: MakeValidator(x)))
                    .ToDictionary(x => x.Type, x => x.ValidatorType);

                return async (ctx) =>
                {
                    foreach (var argument in ctx.Arguments)
                    {
                        // Если аргумент лямбды эндпойнта не имеет валидатора (для него не запрашивалась валидация)
                        if (argument is null
                            || !validatorTypes.TryGetValue(argument.GetType(), out var validatorType))
                            continue;

                        var context = new ValidationContext<object>(argument);
                        var validator = (IValidator) ctx.HttpContext.RequestServices.GetRequiredService(validatorType);
                        var result = await validator.ValidateAsync(context);

                        if (!result.IsValid)
                            return Results.Problem(
                                new ValidationFailedException(
                                        result.Errors
                                            .ToValidationResult())
                                    .ToProblemDetails());
                    }

                    return await next(ctx);
                };
            });
        return builder;
    }

    private static Type MakeValidator(Type typeToValidate)
        => typeof(IValidator<>).MakeGenericType(typeToValidate);
}
