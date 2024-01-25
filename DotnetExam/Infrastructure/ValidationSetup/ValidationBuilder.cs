namespace DotnetExam.Infrastructure.ValidationSetup;

public readonly struct ValidationBuilder<TBuilder> where TBuilder : IEndpointConventionBuilder
{
    private readonly TBuilder _builder;
    private readonly List<Type> _registrations;
    
    public ValidationBuilder(TBuilder builder)
    {
        _builder = builder;
        _registrations = new List<Type>();
    }

    public ValidationBuilder<TBuilder> AddFor<TFor>()
    {
        _registrations.Add(typeof(TFor));
        return this;
    }

    public TBuilder SetValidation()
    {
        _builder.AddValidationFilter(_registrations.ToArray());
        return _builder;
    }
}
